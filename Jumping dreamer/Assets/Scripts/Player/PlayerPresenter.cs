using UnityEngine;

public class PlayerPresenter : SuperMonoBehaviour
{
    public Rigidbody2D PlayerRigidbody2D { get; private set; }

    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerView PlayerView { get; private set; }


    public ScoreCollector ScoreCollector { get; private set; }
    public StarCollector StarCollector { get; private set; }
    public SpeedTrail SpeedTrail { get; private set; }
    public LifeTimer LifeTimer { get; private set; }
    public PlayerTactics PlayerTactics { get; private set; }
    public GravitySensitive GravitySensitive { get; private set; }


    protected override void AwakeWrapped()
    {
        PlayerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        PlayerHealth = gameObject.GetComponent<PlayerHealth>();
        PlayerView = gameObject.GetComponent<PlayerView>();
        PlayerMovement = gameObject.GetComponent<PlayerMovement>();
        ScoreCollector = gameObject.GetComponent<ScoreCollector>();
        StarCollector = gameObject.GetComponent<StarCollector>();
        SpeedTrail = gameObject.GetComponent<SpeedTrail>();
        LifeTimer = gameObject.GetComponent<LifeTimer>();
        PlayerTactics = gameObject.GetComponent<PlayerTactics>();
        GravitySensitive = gameObject.GetComponent<GravitySensitive>();
    }
}
