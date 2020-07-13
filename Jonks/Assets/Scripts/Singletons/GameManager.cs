using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public GameObject Player;
    public PlayerPresenter PlayerPresenter;
    public GameObject Centre;

    public readonly float CentreRadius = 9f;


    protected override void AwakeSingleton()
    {
        base.AwakeSingleton();
        PlayerPresenter = Player.GetComponent<PlayerPresenter>();
    }
}
