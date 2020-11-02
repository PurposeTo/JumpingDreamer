using UnityEngine;

public abstract class RewardCollector : MonoBehaviour
{
    private protected bool canCollecting = true;


    private protected virtual void Start()
    {
        GameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie += StopCollecting;
    }


    private protected virtual void OnDestroy()
    {
        GameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.OnPlayerDie -= StopCollecting;
    }


    private void StopCollecting(bool isPlayerDie)
    {
        canCollecting = !isPlayerDie;
    }
}
