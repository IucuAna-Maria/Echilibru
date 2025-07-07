using UnityEngine;

public class SpellAttack : MonoBehaviour
{
    public Transform spellSpawnPoint;
    public GameObject spellPrefab;
    public float spellSpeed = 10;
    public float spellLifetime = 1f;
    public float fireCooldown = 0.7f;

    private float cooldownTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldownTimer <= 0f)
        {
            ShootSpell();
            cooldownTimer = fireCooldown;
        }
    }

    void ShootSpell()
    {
        float targetingRadius = 15f;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");

        GameObject closestTarget = null;
        float minDistance = targetingRadius;

        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance <= targetingRadius && distance < minDistance)
            {
                minDistance = distance;
                closestTarget = enemy;
            }
        }

        foreach (GameObject boss in bosses)
        {
            float distance = Vector3.Distance(playerPosition, boss.transform.position);
            if (distance <= targetingRadius && distance < minDistance)
            {
                minDistance = distance;
                closestTarget = boss;
            }
        }

        Vector3 direction;

        if (closestTarget != null)
        {
            direction = (closestTarget.transform.position - spellSpawnPoint.position).normalized;
        }
        else
        {
            direction = Camera.main.transform.forward;
            direction.y = 0f;
            direction.Normalize();
        }

        GameObject spell = Instantiate(spellPrefab, spellSpawnPoint.position, Quaternion.LookRotation(direction));

        Rigidbody rb = spell.GetComponent<Rigidbody>();
        rb.AddForce(direction * spellSpeed, ForceMode.VelocityChange);

        Destroy(spell, spellLifetime);
    }
}
