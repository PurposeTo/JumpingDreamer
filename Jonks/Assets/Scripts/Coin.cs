using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    private void OnEnable()
    {
        CoinGenerator.Instance.NumberOfActiveCoins++;
    }


    private void OnDisable()
    {
        CoinGenerator.Instance.NumberOfActiveCoins--;
    }
}
