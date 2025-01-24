using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherPlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rb2d;

        [SerializeField]
        private float movementMultiplier;

        private List<PlayerController> playersInside;

        private Vector2 cachedMultipleVector;

        private void Awake()
        {
            foreach (PlayerController player in playersInside)
            {
                player.OnPlayerMove += OnPlayerMove;
                player.OnPlayerSplit += OnPlayerSplit;
            }
        }

        private void OnDestroy()
        {
            foreach (var player in playersInside)
            {
                player.OnPlayerMove -= OnPlayerMove;
                player.OnPlayerSplit -= OnPlayerSplit;
            }
        }

        public void AddPlayerInside(PlayerController player)
        {
            playersInside.Add(player);
        } 

        private void OnPlayerSplit(PlayerController player)
        {
            playersInside.Remove(player);
        }

        private void OnPlayerMove(Vector2 playerVector)
        {
            cachedMultipleVector += playerVector;
        }

        private void FixedUpdate()
        {
            rb2d.MovePosition(rb2d.position + (movementMultiplier * Time.fixedDeltaTime * cachedMultipleVector));
        }
    }
}
