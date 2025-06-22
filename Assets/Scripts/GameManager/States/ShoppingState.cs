using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingState : BaseState<EGameState>
{
    public float Timer { get; private set; }
    GameManager gameManager;
    Slider _countDownSlider;
    public ShoppingState(GameManager manager) : base(EGameState.Shopping)
    {
        gameManager = manager;
        LoadSlider();
    }

    private async void LoadSlider()
    {
        while (!UIManager.Instance)
        {
            await Task.Delay(100);
        }

        while (!_countDownSlider)
        {
            var panel = UIManager.Instance.GetPanel<GameplayMainPanel>(UIConstant.GameplayPanel);
            
            if (panel)
            {
                _countDownSlider = panel.CountDownSlider;
            }
            await Task.Delay(100);
        }
    }

    public override void Enter()
    {
        GameEvent.OnStartShoppingState?.Invoke();
        gameManager.SkipShopping = false;
        Timer = gameManager.ShoppingTime;
    }

    public override void Exit()
    {
        GameEvent.OnStopShoppingState?.Invoke();
        gameManager.SkipShopping = false;
    }

    public override void FixedUpdate()
    {

    }

    public override EGameState GetNextState()
    {
        if(Timer <= 0 || gameManager.SkipShopping)
        {
            return EGameState.Combat;
        }
        return Key;
    }

    public override void Update()
    {
        if (_countDownSlider)
        {
            _countDownSlider.value = Timer/gameManager.ShoppingTime;
        }
        Timer -= Time.deltaTime;
    }
}
