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

        private List<PlayerBubbleContext> playersInside = new();

        private void Subscribe(PlayerController player)
        {
            player.OnPlayerSplit += OnPlayerSplit;
        }

        private void Unsubscribe(PlayerController player)
        {
            player.OnPlayerSplit -= OnPlayerSplit;
        }

        public void AddPlayerInside(PlayerBubbleContext playerContext)
        {
            if (playersInside.Contains(playerContext))
            {
                return;
            }
            playersInside.Add(playerContext);
            Subscribe(playerContext.PlayerController);

            motherBubbleContext.BubbleWeightSystem.AddWeight(playerContext.BubbleWeightSystem.Weight);

            playerContext.PlayerController.ChangeMovement(false);
            playerContext.Transform.SetParent(rb2d.transform, false);
            playerContext.Transform.localPosition = Vector2.zero;
        } 

        private void OnPlayerSplit(PlayerBubbleContext playerContext)
        {
            if (!playersInside.Contains(playerContext))
            {
                return;
            }
            Unsubscribe(playerContext.PlayerController);
            playersInside.Remove(playerContext);

            motherBubbleContext.BubbleWeightSystem.RemoveWeight(playerContext.BubbleWeightSystem.Weight);

            playerContext.PlayerController.ChangeMovement(true);
            playerContext.Transform.SetParent(null);
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
