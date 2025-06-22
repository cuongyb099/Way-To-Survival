using TMPro;
using UnityEngine;

public class ShopPanel : FadeBlurPanel
{
    public TextMeshProUGUI CashText;
    public GameObject BuffPanel;

    public int BuffPrice = 3;
    private PlayerController player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    public override void Show()
    {
        base.Show();
        PlayerEvent.OnCashChange += UpdateCashText;
        UpdateCashText(player.Resin);
    }

    public override void Hide()
    {
        base.Hide();
        PlayerEvent.OnCashChange -= UpdateCashText;
    }

    private void UpdateCashText(int amount)
    {
        CashText.text = $"<color=#3ECDFF>Cash:</color> {amount}";
    }

    public void OnBuyBuff()
    {
        if (player.Resin < BuffPrice)
        {
            Debug.Log("Not enough cash");
            return;
        }
        player.Resin -= BuffPrice;
        BuffPanel.SetActive(true);
    }
}
