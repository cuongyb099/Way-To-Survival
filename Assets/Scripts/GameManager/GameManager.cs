
using Cysharp.Threading.Tasks;
using FidelityFX;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EGameState
{
    Shopping,
    Combat,
    WaveWon,
    Died,
}
public class GameManager : StateMachine<EGameState>
{
    public PlayerController Player { get; private set; }
    public WaveManager WaveManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }
    [field:SerializeField] public AudioClip heliSound{ get; private set; }
    [field: Header("Game Variables")]
    [field:SerializeField] public int WaveWonTime{ get; private set; } = 30;
    [field:SerializeField] public float ShoppingTime{ get; private set; } = 30f;
    [field: Header("Game Settings Variables")]
    [field:SerializeField] public Fsr3UpscalerImageEffect Fsr{ get; private set; }
    public bool SkipShopping { get; set; } = false;
    //Singleton
    public static GameManager Instance { get; private set; } = null;
    
    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        InitializeUIAsync().Forget();

        Player = FindAnyObjectByType<PlayerController>();
        WaveManager = FindAnyObjectByType<WaveManager>();
        EnemyManager = FindAnyObjectByType<EnemyManager>();
        
        States.Add(EGameState.Shopping, new ShoppingState(this));
        States.Add(EGameState.Combat, new CombatState(this));
        States.Add(EGameState.WaveWon, new WaveWonState(this));
        States.Add(EGameState.Died, new DiedState(this));
    }

    private async UniTaskVoid InitializeUIAsync()
    {
        GameEvent.OnInitializedUI?.Invoke();
    }

    private void Start()
    {
        CurrentState = States[EGameState.Shopping];
        TransitionToState(EGameState.Shopping);
        AudioManager.Instance.PlaySound(heliSound);
    }


    public void ChangeGameState(EGameState newGameState)
    {
        TransitionToState(newGameState);
    }

    [ContextMenu("Skip CombatState")]
    public void SkipCombatState()
    {
        if(CurrentState != States[EGameState.Combat]) return;
        TransitionToState(EGameState.WaveWon);
    }
}
