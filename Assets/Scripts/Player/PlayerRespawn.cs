using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject deathScreen;
    public GameObject defaultRespawnPoint;
    private GameObject currentRespawnPoint;
    public GameObject playerMesh;
    public GameObject playerArmor;

    private MonoBehaviour spellAttackScript;
    private PlayerHealth playerHealth;
    private CharacterController characterController;
    private PlayerLocomotionInput locomotionInput;
    private PlayerActionsInput actionsInput;

    private void Start()
    {
        currentRespawnPoint = defaultRespawnPoint;

        playerHealth = GetComponent<PlayerHealth>();
        characterController = GetComponent<CharacterController>();
        locomotionInput = GetComponent<PlayerLocomotionInput>();
        actionsInput = GetComponent<PlayerActionsInput>();
        spellAttackScript = GetComponent<SpellAttack>();

        if (deathScreen != null)
            deathScreen.SetActive(false);
    }

    public void SetCheckpoint(GameObject checkpoint)
    {
        currentRespawnPoint = checkpoint;
        Debug.Log($"Checkpoint set: {checkpoint.name}");
    }

    public void Respawn()
    {
        StartCoroutine(HandleRespawn());
    }

    private IEnumerator HandleRespawn()
    {
        if (playerMesh != null)
            playerMesh.SetActive(false);
        if (playerArmor != null)
            playerArmor.SetActive(false);

        if (locomotionInput != null) locomotionInput.DisableInput();
        if (actionsInput != null) actionsInput.DisableInput();

        if (spellAttackScript != null)
            spellAttackScript.enabled = false;

        if (deathScreen != null)
            deathScreen.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (characterController != null)
            characterController.enabled = false;

        if (currentRespawnPoint != null)
            transform.position = currentRespawnPoint.transform.position;

        if (characterController != null)
            characterController.enabled = true;

        if (playerMesh != null)
            playerMesh.SetActive(true);
        if (playerArmor != null)
            playerArmor.SetActive(true);

        if (locomotionInput != null) locomotionInput.EnableInput();
        if (actionsInput != null) actionsInput.EnableInput();

        if (spellAttackScript != null)
            spellAttackScript.enabled = true;

        if (deathScreen != null)
            deathScreen.SetActive(false);

        if (playerHealth != null)
            playerHealth.currentHealth = playerHealth.maxHealth;
    }
}
