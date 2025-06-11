using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int baseHealth = 100;
    public int currentHealth;
    private int appliedBonusHP = 0;

    private bool initialized = false;

    public void InitializeBossStats()
    {
        int currentBonus = BossManager.Instance != null ? BossManager.Instance.GetBonusHP() : 0;

        if (!initialized || currentBonus != appliedBonusHP)
        {
            appliedBonusHP = currentBonus;
            currentHealth = baseHealth + appliedBonusHP;
            initialized = true;
        }
    }

    public void ResetHealthToMax()
    {
        initialized = false;
        InitializeBossStats();
    }

    public void TakeDamage(int amount)
    {
        InitializeBossStats();

        PlayerCombatTracker tracker = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombatTracker>();
        if (tracker != null && tracker.HasBonus())
        {
            amount += 20;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (BossManager.Instance != null)
            BossManager.Instance.RegisterBossDefeat();

        Destroy(gameObject);
    }
}
