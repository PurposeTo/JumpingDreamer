﻿using Assets.Scripts.Player.Data;
using UnityEngine;

namespace Assets.Scripts.Player.Statistics
{
    class JumpHeightStats : MonoBehaviour
    {
        private Rigidbody2D playerRb2D;
        private GameObject centre;

        private float jumpHeight = 0f;


        private void Start()
        {
            playerRb2D = gameObject.GetComponent<Rigidbody2D>();
            centre = GameManager.Instance.Centre;

            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveJumpHeightStats;
        }


        private void OnDestroy()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveJumpHeightStats;
        }


        private void FixedUpdate()
        {
            Vector2 toCentreVector = (Vector2)centre.transform.position - playerRb2D.position;

            // Если вектор скорости игрока относительно центра направлен вверх
            if (Vector3.Project(playerRb2D.velocity, toCentreVector).magnitude <= 0f)
            {
                float currentJumpHeight = -1 * (toCentreVector.magnitude - GameManager.Instance.CentreRadius);

                if (currentJumpHeight > jumpHeight)
                {
                    jumpHeight = currentJumpHeight;
                }
            }
        }

        private void SaveJumpHeightStats()
        {
            PlayerStatsDataStorageSafe.Instance.SaveJumpHeightData(jumpHeight);
        }
    }
}
