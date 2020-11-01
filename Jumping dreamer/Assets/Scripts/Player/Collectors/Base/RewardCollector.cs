using UnityEngine;

public abstract class RewardCollector : MonoBehaviour
{
    private protected bool canCollecting = true;


    private protected virtual void Start()
    {
        ImportantGameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie += StopCollecting;
    }


    private protected virtual void OnDestroy()
    {
        ImportantGameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie -= StopCollecting;
    }


    private void StopCollecting(bool isPlayerDie)
    {
        canCollecting = !isPlayerDie;
    }
}
