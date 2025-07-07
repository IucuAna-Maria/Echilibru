using UnityEngine;

public class PlayerCombatTracker : MonoBehaviour
{
    public int enemiesKilled = 0;
    public bool hasBossDamageBonus = false;
    public int killThreshold = 5;

    public void RegisterKill()
    {
        enemiesKilled++;

        if (enemiesKilled >= killThreshold && !hasBossDamageBonus)
        {
            hasBossDamageBonus = true;
        }
    }

    public void ResetKills()
    {
        enemiesKilled = 0;
        hasBossDamageBonus = false;
    }

    public bool HasBonus()
    {
        return hasBossDamageBonus;
    }
}
