using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public ScoreCollector ScoreCollector { get; private set; }
    public StarCollector StarCollector { get; private set; }
    public SpeedTrail SpeedTrail { get; private set; }
    public LifeTimer LifeTimer { get; private set; }
    public PlayerTactics PlayerTactics { get; private set; }
    public GravitySensitive GravitySensitive { get; private set; }


    private void Awake()
    {
        PlayerHealth = gameObject.GetComponent<PlayerHealth>();
        PlayerMovement = gameObject.GetComponent<PlayerMovement>();
        ScoreCollector = gameObject.GetComponent<ScoreCollector>();
        StarCollector = gameObject.GetComponent<StarCollector>();
        SpeedTrail = gameObject.GetComponent<SpeedTrail>();
        LifeTimer = gameObject.GetComponent<LifeTimer>();
        PlayerTactics = gameObject.GetComponent<PlayerTactics>();
        GravitySensitive = gameObject.GetComponent<GravitySensitive>();
    }
}
