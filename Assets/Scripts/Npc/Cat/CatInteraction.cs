using UnityEngine;

public class CatInteraction : MonoBehaviour
{
    public GameObject messageUI;
    public GameObject fullMenuUI;
    public float messageDuration = 2f;

    private bool playerNearby = false;
    private bool fullMenuShown = false;
    private float timer = 0f;
    private bool messageActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        messageUI.SetActive(false);
        fullMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E) && !messageActive && !fullMenuShown)
        {
            ShowMessage();
        }

        if (messageActive)
        {
            timer += Time.deltaTime;

            if (timer >= messageDuration)
            {
                messageUI.SetActive(false);
                messageActive = false;

                if (playerNearby)
                {
                    fullMenuUI.SetActive(true);
                    fullMenuShown = true;
                }
            }
        }
    }

    private void ShowMessage()
    {
        messageUI.SetActive(true);
        timer = 0f;
        messageActive = true;
        fullMenuShown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            messageUI.SetActive(false);
            messageActive = false;

            if (fullMenuShown)
            {
                fullMenuUI.SetActive(false);
                fullMenuShown = false;
            }
        }
    }

    public void OnMenuClosed()
    {
        fullMenuShown = false;
        messageActive = false;
        timer = 0f;
    }
}
