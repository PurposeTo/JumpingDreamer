﻿using Assets.Scripts.Player.Data;
using UnityEngine;
using System;

[Obsolete]
class JumpHeightStats : MonoBehaviour
{
    private Rigidbody2D playerRb2D;
    private GameObject centre;

    private float jumpHeight = 0f;


    private void Start()
    {
        playerRb2D = gameObject.GetComponent<Rigidbody2D>();
        centre = GameManager.Instance.Centre;

        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats += SaveJumpHeightStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats -= SaveJumpHeightStats;
    }


    private void FixedUpdate()
    {
        Vector2 toCentreVector = (Vector2)centre.transform.position - playerRb2D.position;

        // Если вектор скорости игрока относительно центра направлен вверх И если игрок не касается платформы
        float velocityOnAxisCentreProject = Vector3.Project(playerRb2D.velocity, toCentreVector).magnitude;

        if (velocityOnAxisCentreProject >= 0f)
        {
            float currentJumpHeight = toCentreVector.magnitude - Centre.CentreRadius;

            if (currentJumpHeight > jumpHeight)
            {
                jumpHeight = currentJumpHeight;
            }
        }
    }

    private void SaveJumpHeightStats()
    {
        PlayerStatsDataStorageSafe.Instance.SaveJumpHeightData((float)Math.Round(jumpHeight, 1));
    }
}
