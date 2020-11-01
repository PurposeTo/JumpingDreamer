using UnityEngine;

public abstract class RewardCollector : MonoBehaviour
{
    private protected bool canCollecting = true;


    private protected virtual void Start()
    {
        GameManager.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie += StopCollecting;
    }


    private protected virtual void OnDestroy()
    {
        GameManager.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie -= StopCollecting;
    }


    private void StopCollecting(bool isPlayerDie)
    {
        canCollecting = !isPlayerDie;
    }
}
