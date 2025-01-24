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

        public Rigidbody2D Rb2D => rb2d;

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

            player.ChangeMovement(false);
            player.transform.SetParent(this.Rb2D.transform, false);
            player.transform.localPosition = Vector2.zero;
        } 

        private void OnPlayerSplit(PlayerController player)
        {
            Unsubscribe(player);
            playersInside.Remove(player);

            player.ChangeMovement(true);
            player.transform.SetParent(null);
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
