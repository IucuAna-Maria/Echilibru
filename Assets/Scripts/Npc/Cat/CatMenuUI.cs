using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatMenuUI : MonoBehaviour
{
    public CatInteraction catInteraction;
    public PlayerHealth playerHealth;

    [Header("Armor Purchase")]
    private int armorCost = 10;
    public GameObject chestPlate;
    public GameObject gloves;
    public GameObject helmet;
    public GameObject pants;
    public GameObject shoulders;
    private bool armorPurchased = false;

    [Header("UI Feedback")]
    public GameObject feedbackText;
    public Button armorButton;

    public void CloseMenu()
    {
        gameObject.SetActive(false);

        if (catInteraction != null)
            catInteraction.OnMenuClosed();
    }

    public void IncreaseHealthButton()
    {
        if (CoinManager.Instance != null && playerHealth != null)
        {
            if (CoinManager.Instance.HasEnough(30))
            {
                CoinManager.Instance.SpendCoin(30);
                playerHealth.IncreaseMaxHealth(10);
            }
            else 
            {
                ShowInsufficientFunds();
            }
        }
    }

    public void BuyArmor()
    {
        if (armorPurchased) return;

        if (CoinManager.Instance != null && CoinManager.Instance.HasEnough(armorCost))
        {
            CoinManager.Instance.SpendCoin(armorCost);

            if (chestPlate != null) chestPlate.SetActive(true);
            if (gloves != null) gloves.SetActive(true);
            if (helmet != null) helmet.SetActive(true);
            if (pants != null) pants.SetActive(true);
            if (shoulders != null) shoulders.SetActive(true);

            playerHealth.hasArmor = true;

            armorPurchased = true;

            if (armorButton != null)
                armorButton.interactable = false;
        }
        else
        {
            ShowInsufficientFunds();
        }
    }

    private void ShowInsufficientFunds()
    {
        if (feedbackText != null)
        { 
            feedbackText.SetActive(true);
            StartCoroutine(HideFeedbackAfterDelay(1.5f));
        }
    }

    private IEnumerator HideFeedbackAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (feedbackText != null)
            feedbackText.SetActive(false);
    }
}