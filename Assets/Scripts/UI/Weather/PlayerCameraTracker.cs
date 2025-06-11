using UnityEngine;

public class PlayerCameraTracker : MonoBehaviour
{
    public Transform playerCamera;
    public float yoffset = 15f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = playerCamera.position;
        newPosition.y = yoffset;

        this.transform.position = newPosition;
    }
}
