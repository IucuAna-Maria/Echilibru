using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    public float lookRadius = 20f;

    [Header("Projectile Attack")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;
    public float attackCooldown = 2f;

    private float attackTimer = 0f;

    Transform target;
    NavMeshAgent agent;

    private PlayerHealth playerHealth;
    private BossHealth bossHealth;

    private bool wasPlayerInRangeLastFrame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        bossHealth = GetComponent<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null || playerHealth.currentHealth <= 0)
        {
            agent.ResetPath();
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        attackTimer -= Time.deltaTime;

        if (distance <= lookRadius)
        {
            bossHealth.InitializeBossStats();
            agent.SetDestination(target.position);

            if (distance <= 10f)
            {
                FaceTarget();

                if (attackTimer <= 0f)
                {
                    ShootProjectile();
                    attackTimer = attackCooldown;
                }
            }
            wasPlayerInRangeLastFrame = true;
        }
        else
        {
            if (wasPlayerInRangeLastFrame)
            {
                bossHealth.ResetHealthToMax();
                wasPlayerInRangeLastFrame = false;
            }

            agent.ResetPath();
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Destroy(proj, 5f);

            Vector3 targetPosition = target.position + Vector3.up * 1.3f;
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;

            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction * projectileSpeed, ForceMode.VelocityChange);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
