using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coinCount = 0;
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    public void SpendCoin(int amount)
    {
        coinCount -= amount;
        UpdateUI();
    }

    public bool HasEnough(int amount)
    {
        return coinCount >= amount;
    }

    public void ResetCoins()
    {
        coinCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        coinText.text = "" + coinCount;
    }
}
