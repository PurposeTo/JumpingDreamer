using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public GameObject Player;
    public PlayerPresenter PlayerPresenter { get; private set; }
    public GameObject CentreObject;
    public Centre Centre { get; private set; }

    public GameObject CameraObject;


    protected override void AwakeSingleton()
    {
        PlayerPresenter = Player.GetComponent<PlayerPresenter>();
        Centre = CentreObject.GetComponent<Centre>();
    }
}
