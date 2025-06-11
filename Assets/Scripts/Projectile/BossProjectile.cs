using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int baseDamage = 20;
    private int damage;

    void Start()
    {
        if (BossManager.Instance != null)
            damage = baseDamage + BossManager.Instance.GetBonusDamage();
        else
            damage = baseDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
