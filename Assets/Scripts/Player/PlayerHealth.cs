using System;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;

    public float healInterval = 1f;
    public int healAmount = 2;
    public int healCoinThreshold = 50;

    private float healTimer = 0f;
    private float errorCheckTimer = 0f;
    private float errorCheckInterval = 2f;

    public HealthBar healthBar;
    private PlayerRespawn respawnScript;
    public GameObject errorImage;

    public bool hasArmor = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        respawnScript = GetComponent<PlayerRespawn>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHealth);
        HandlePassiveHealing();
        HandleErrorPopup();
    }

    private void HandlePassiveHealing()
    {
        if (CoinManager.Instance != null && CoinManager.Instance.coinCount >= healCoinThreshold)
        {
            if (currentHealth < maxHealth)
            {
                healTimer += Time.deltaTime;

                if (healTimer >= healInterval)
                {
                    currentHealth += healAmount;
                    currentHealth = Mathf.Min(currentHealth, maxHealth);
                    healTimer = 0f;
                }
            }
        }
        else
        {
            healTimer = 0f;
        }
    }

    private void HandleErrorPopup()
    {
        float hpPercent = (float)currentHealth / maxHealth;

        if (currentHealth <= 0) return;

        if (hpPercent <= 0.2f && errorImage != null && !errorImage.activeSelf)
        {
            errorCheckTimer += Time.deltaTime;

            if (errorCheckTimer >= errorCheckInterval)
            {
                errorCheckTimer = 0f;

                int chance = UnityEngine.Random.Range(0, 100);
                Debug.Log("Chance value: " + chance);

                if (chance < 30)
                {
                    StartCoroutine(ShowErrorImageTemporary());
                }
            }
        }
        else
        {
            errorCheckTimer = 0f;
        }
    }

    private IEnumerator ShowErrorImageTemporary()
    {
        errorImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        errorImage.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (hasArmor)
        {
            damage = Mathf.RoundToInt(damage * 0.8f);
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        PlayerCombatTracker tracker = GetComponent<PlayerCombatTracker>();
        if (tracker != null)
        {
            tracker.ResetKills();
        }

        if (errorImage != null && errorImage.activeSelf)
        {
            errorImage.SetActive(false);
        }

        BossHealth[] allBosses = FindObjectsByType<BossHealth>(FindObjectsSortMode.None);
        foreach (var boss in allBosses)
        {
            boss.ResetHealthToMax();
        }

        if (respawnScript != null)
        {
            CoinManager.Instance.ResetCoins();
            respawnScript.Respawn();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        
        Debug.Log($"Max HP increased to: {maxHealth}");
    }
}
