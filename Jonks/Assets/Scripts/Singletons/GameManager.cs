using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public GameObject Player;
    public PlayerPresenter PlayerPresenter { get; private set; }
    public GameObject Centre;

    protected override void AwakeSingleton()
    {
        base.AwakeSingleton();
        PlayerPresenter = Player.GetComponent<PlayerPresenter>();
    }
}
