using ResilientCore;

public class DiedState : BaseState<EGameState>
{
    GameManager gameManager;
    public DiedState(GameManager manager) : base(EGameState.Died)
    {
        gameManager = manager;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override EGameState GetNextState()
    {
        return Key;
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }
}
