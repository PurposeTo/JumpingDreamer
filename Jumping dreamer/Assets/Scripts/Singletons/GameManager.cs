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


    public Vector2 GetToCentreVector(Vector2 position)
    {
        return (Vector2)CentreObject.transform.position - position;
    }


    public Vector2 GetToCentreDirection(Vector2 position)
    {
        return GetToCentreVector(position).normalized;
    }


    public float GetToCentreMagnitude(Vector2 position)
    {
        return GetToCentreVector(position).magnitude;
    }


    public Vector2 GetFromCentreVector(Vector2 position)
    {
        return GetToCentreVector(position) * -1f;
    }


    public Vector2 GetFromCentreDirection(Vector2 position)
    {
        return GetToCentreDirection(position) * -1f;
    }


    public float GetFromCentreMagnitude(Vector2 position)
    {
        return GetToCentreMagnitude(position) * -1f;
    }
}
