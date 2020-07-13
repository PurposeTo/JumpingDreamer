using System;
using UnityEngine;

[Obsolete]
public class ExtraSizer : MonoBehaviour
{
    private GameObject player;
    private float toPlayerDistance;

    private float defaulSize;
    private float extraSize;
    private float extraSizeMultilpy = 0.1f;
    private float speed = 0.5f;


    private void Start()
    {
        player = GameManager.Instance.Player;
        defaulSize = transform.localScale.x;
    }


    private void Update()
    {
        toPlayerDistance = (player.transform.position - transform.position).magnitude;

        extraSize = toPlayerDistance * extraSizeMultilpy;

        float Size = defaulSize + extraSize;
        Size = Mathf.Lerp(transform.localScale.x, Size, speed);

        transform.localScale = new Vector3(Size, Size, Size);
    }
}
