using UnityEngine;
using UnityEngine.UI;

public class PlayerCompass : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer sprite;

    private float MinHighest => MaxHighest * 2f / 3f;
    private float MaxHighest => PlatformGeneratorData.PlatformAvailableHighestArea;

    private float alphaChanel;


    private void Start()
    {
        player = GameManager.Instance.Player;
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        float currentRange = GameManager.Instance.GetToCentreMagnitude(player.transform.position);

        alphaChanel = GameLogic.PercentLerp(MinHighest, MaxHighest, currentRange);

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3f, player.transform.position.z);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alphaChanel);
    }
}
