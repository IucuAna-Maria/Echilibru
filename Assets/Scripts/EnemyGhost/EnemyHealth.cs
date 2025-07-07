using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Health = 50;
    public PlayerCombatTracker playerTracker;

    public void TakeDamage(int amount)
    {
        Health -= amount;

        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (playerTracker != null)
        {
            playerTracker.RegisterKill();
        }

        Destroy(gameObject);
    }
}
