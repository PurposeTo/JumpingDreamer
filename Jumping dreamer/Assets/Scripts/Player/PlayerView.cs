using System.Collections;
using System.Collections.Generic;
using Desdiene.Super_monoBehaviour;
using UnityEngine;

public class PlayerView : SuperMonoBehaviour
{
    private PlayerMovement playerMovement;

    private float scaleX = 1f;

    protected override void AwakeWrapped()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    protected override void UpdateWrapped()
    {
        scaleX = Mathf.Approximately(playerMovement.HorizontalInput, 0f)
            ? scaleX
            : playerMovement.HorizontalInput > 0f
                ? -1f
                : 1f;

        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }
}
