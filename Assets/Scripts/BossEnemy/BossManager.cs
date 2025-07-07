using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    [Header("Progressive scaling")]
    public int bossesDefeated = 0;
    public int hpIncreasePerBoss = 20;
    public int damageIncreasePerBoss = 5;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public int GetBonusHP()
    {
        return bossesDefeated * hpIncreasePerBoss;
    }

    public int GetBonusDamage()
    {
        return bossesDefeated * damageIncreasePerBoss;
    }

    public void RegisterBossDefeat()
    {
        bossesDefeated++;
        Debug.Log("Boss total: " + bossesDefeated);
    }
}
