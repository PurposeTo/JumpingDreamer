using UnityEngine;

[RequireComponent(typeof(GPGSAuthentication))]
[RequireComponent(typeof(GPGSLeaderboard))]
[RequireComponent(typeof(GPGSPlayerDataCloudStorage))]
public class GPGSServices : SingletonMonoBehaviour<GPGSServices>
{
    public GPGSAuthentication GPGSAuthentication { get; private set; }
    public GPGSLeaderboard GPGSLeaderboard { get; private set; }
    public GPGSPlayerDataCloudStorage GPGSPlayerDataCloudStorage { get; private set; }


    protected override void AwakeSingleton()
    {
        GPGSAuthentication = gameObject.GetComponent<GPGSAuthentication>();
        GPGSLeaderboard = gameObject.GetComponent<GPGSLeaderboard>();
        GPGSPlayerDataCloudStorage = gameObject.GetComponent<GPGSPlayerDataCloudStorage>();
    }
}
