using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.AddCoin(coinValue);
            Destroy(gameObject);
        }
    }
}
