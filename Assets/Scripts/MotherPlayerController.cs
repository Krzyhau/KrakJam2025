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

        [SerializeField]
        private MotherBubbleContext motherBubbleContext;

        public Rigidbody2D Rb2D => rb2d;

        private List<PlayerBubbleContext> playersInside = new();

        private void Subscribe(PlayerController player)
        {
            player.OnPlayerSplit += OnPlayerSplit;
        }

        private void Unsubscribe(PlayerController player)
        {
            player.OnPlayerSplit -= OnPlayerSplit;
        }

        public void AddPlayerInside(PlayerBubbleContext player)
        {
            if (playersInside.Contains(player))
            {
                return;
            }
            playersInside.Add(player);
            Subscribe(player.PlayerController);

            motherBubbleContext.BubbleWeightSystem.AddWeight(player.BubbleWeightSystem.Weight);

            player.PlayerController.ChangeMovement(false);
            player.transform.SetParent(this.Rb2D.transform, false);
            player.transform.localPosition = Vector2.zero;
        } 

        private void OnPlayerSplit(PlayerBubbleContext player)
        {
            if (!playersInside.Contains(player))
            {
                return;
            }
            Unsubscribe(player.PlayerController);
            playersInside.Remove(player);

            player.PlayerController.ChangeMovement(true);
            player.transform.SetParent(null);
        }

        private void FixedUpdate()
        {
            if (playersInside != null && playersInside.Count > 0)
            {
                float totalInputX = playersInside.Sum(x => x.PlayerController.CachedInput.x);
                float totalInputY = playersInside.Sum(x => x.PlayerController.CachedInput.y);
                Vector2 totalInput = new(totalInputX, totalInputY);
                rb2d.AddForce(movementMultiplier * Time.fixedDeltaTime * totalInput, ForceMode2D.Impulse);
            }
        }
    }
}
