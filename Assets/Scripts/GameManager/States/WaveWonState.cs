
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WaveWonState : BaseState<EGameState>
{
    public float Timer { get; private set; }
    GameManager gameManager;
    Slider _countDownSlider;
    public WaveWonState(GameManager manager) : base(EGameState.WaveWon)
    {
        gameManager = manager;
    }

    private async void LoadSlider()
    {
        while (!UIManager.Instance)
        {
            await Task.Delay(100);
        }

        while (!_countDownSlider)
        {
            _countDownSlider = UIManager.Instance
                .GetPanel<GameplayMainPanel>(UIConstant.GameplayPanel)
                .CountDownSlider;
            await Task.Delay(100);
        }
    }
    

    public override void Enter()
    {
        GameEvent.OnStartWinState?.Invoke();
        Timer = gameManager.WaveWonTime;
        UIManager.Instance.HidePanel(UIConstant.GameplayPanel);
        UIManager.Instance.ShowPanel(UIConstant.BuffPanel);
        TimeManager.Instance.AdvanceTimeOfDay();
    }

    public override void Exit()
    {
        GameEvent.OnStopWaveWinState?.Invoke();
    }

    public override void FixedUpdate()
    {
        
    }

    public override EGameState GetNextState()
    {
        if(Timer <= 0)
        {
            return EGameState.Shopping;
        }
        return Key;
    }

    public override void Update()
    {
        if (_countDownSlider)
        {
            _countDownSlider.value = Timer/gameManager.WaveWonTime;
        }
        Timer -= Time.deltaTime;
    }

}
