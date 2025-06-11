using UnityEngine;

public class LoudnessEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float loudnessThreshold = 0.020f;
    public float cooldown = 2f;
    public float spawnDistance = 20f;

    private AudioClip micInput;
    private string micName;
    private float cooldownTimer = 0f;

    int numberOfPoints = 6;
    int currentSpawnIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micInput = Microphone.Start(micName, true, 10, 44100);
        }
        else
        {
            Debug.LogWarning("No microphone found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Microphone.IsRecording(micName))
        {
            Debug.LogWarning("Microphone disconnected. Trying to reconnect...");
            TryReconnectMicrophone();
            return;
        }

        cooldownTimer -= Time.deltaTime;

        float loudness = GetLoudnessFromMic();

        //Debug.Log($"Loudness: {loudness:F3}");

        if (loudness > loudnessThreshold && cooldownTimer <= 0f)
        {
            SpawnEnemy();
            cooldownTimer = cooldown;
        }
    }

    void TryReconnectMicrophone()
    {
        Microphone.End(micName);

        if (Microphone.devices.Length > 0)
        {
            micName = Microphone.devices[0];
            micInput = Microphone.Start(micName, true, 10, 44100);
            Debug.Log("Microphone reconnected to: " + micName);
        }
        else
        {
            Debug.LogError("No microphone available for reconnection.");
        }
    }

    float GetLoudnessFromMic()
    {
        int sampleWindow = 128;
        float[] data = new float[sampleWindow];
        int position = Microphone.GetPosition(micName) - sampleWindow + 1;

        if (position < 0) return 0;

        micInput.GetData(data, position);

        float sum = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += data[i] * data[i];
        }

        return Mathf.Sqrt(sum / sampleWindow);
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || Camera.main == null)
            return;

        Transform cam = Camera.main.transform;
        Vector3 playerPosition = cam.position;

        bool spawned = false;

        for (int i = 0; i < numberOfPoints; i++)
        {
            int indexToTry = (currentSpawnIndex + i) % numberOfPoints;

            float angleStep = 360f / numberOfPoints;
            float angle = indexToTry * angleStep;
            float radians = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * spawnDistance;
            Vector3 rawSpawnPos = playerPosition + offset;

            if (Physics.Raycast(rawSpawnPos + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f, LayerMask.GetMask("Default")))
            {
                Vector3 finalPos = hit.point;
                GameObject enemy = Instantiate(enemyPrefab, finalPos, Quaternion.identity);

                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    PlayerCombatTracker tracker = player.GetComponent<PlayerCombatTracker>();
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                    if (enemyHealth != null && tracker != null)
                    {
                        enemyHealth.playerTracker = tracker;
                    }
                }

                Debug.Log($"Spawned enemy at index {indexToTry} | angle {angle}°");
                currentSpawnIndex = (indexToTry + 1) % numberOfPoints;
                spawned = true;
                break;
            }
            else
            {
                Debug.Log($"Invalid spawn at index {indexToTry} | angle {angle}°");
            }
        }

        if (!spawned)
        {
            Debug.LogWarning("No valid spawn positions found around player.");
        }
    }
}
