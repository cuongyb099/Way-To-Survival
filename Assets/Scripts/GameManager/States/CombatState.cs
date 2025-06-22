
using ResilientCore;

public class CombatState : BaseState<EGameState>
{
    GameManager gameManager;
    bool exitState = false;
    public CombatState(GameManager manager) : base(EGameState.Combat)
    {
        gameManager = manager;
        
        GameEvent.WaveDoneEvent += WaveWonTrigger;
    }

    public override void Enter()
    {
        GameEvent.OnStartCombatState?.Invoke();
        exitState = false;
    }

    public override void Exit()
    {
        GameEvent.OnStopCombatState?.Invoke();
    }

    public override EGameState GetNextState()
    {

        if (gameManager.Player.IsDead)
        {
            return EGameState.Died;
        }
        if (exitState)
        {
            return EGameState.WaveWon;
        }

        return Key;
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }
    
    public void WaveWonTrigger(int x)
    {
        exitState = true;
    }
    // public void StartShopping()
    // {
    //     //Sample 29 -> 0 30 Number In 30 Seconds
    //     _shoppingTimeTween = DOVirtual.Float(_shoppingTime - 1, 0, _shoppingTime, _timerCallback)
    //         .SetEase(Ease.Linear)
    //         .OnKill(_tweenCallbackNextWave);
    // }
}
