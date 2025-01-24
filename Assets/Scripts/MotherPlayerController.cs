using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherPlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rb2d;

        [SerializeField]
        private float movementMultiplier;

        private List<PlayerController> playersInside = new();

        private Vector2 cachedMultipleVector;

        private void Subscribe(PlayerController player)
        {
            player.OnPlayerSplit += OnPlayerSplit;
        }

        private void Unsubscribe(PlayerController player)
        {
            player.OnPlayerSplit -= OnPlayerSplit;
        }

        public void AddPlayerInside(PlayerController player)
        {
            playersInside.Add(player);
            Subscribe(player);
        } 

        private void OnPlayerSplit(PlayerController player)
        {
            playersInside.Remove(player);
            Unsubscribe(player);
        }

        private void OnPlayerMove(Vector2 playerVector)
        {
            cachedMultipleVector += playerVector;
        }

        private void FixedUpdate()
        {
            if (playersInside != null && playersInside.Count > 0)
            {
                float totalInputX = playersInside.Sum(x => x.CachedInput.x);
                float totalInputY = playersInside.Sum(x => x.CachedInput.y);
                Vector2 totalInput = new(totalInputX, totalInputY);
                rb2d.AddForce(movementMultiplier * Time.fixedDeltaTime * totalInput, ForceMode2D.Impulse);
            }
        }
    }
}
