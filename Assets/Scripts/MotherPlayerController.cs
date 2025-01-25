using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherPlayerController : MonoBehaviour
    {
        public event Action<PlayerBubbleContext> OnPlayerAbsorbed;
        public event Action<PlayerBubbleContext> OnPlayerSplitted;

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
            Subscribe(playerContext.Input);

            Debug.Log(playerContext.WeightSystem.Weight);
            // motherBubbleContext.WeightSystem.AddWeight(playerContext.WeightSystem.Weight);

            playerContext.Input.ChangeMovement(false);
            playerContext.Transform.SetParent(rb2d.transform, false);
            playerContext.Transform.localPosition = Vector2.zero;

            OnPlayerAbsorbed?.Invoke(playerContext);
        } 

        private void OnPlayerSplit(PlayerBubbleContext playerContext)
        {
            if (!playersInside.Contains(playerContext))
            {
                return;
            }
            Unsubscribe(playerContext.Input);
            playersInside.Remove(playerContext);

            // motherBubbleContext.WeightSystem.RemoveWeight(playerContext.WeightSystem.Weight);

            playerContext.Input.ChangeMovement(true);
            playerContext.Transform.SetParent(null);

            OnPlayerSplitted?.Invoke(playerContext);
        }

        private void FixedUpdate()
        {
            if (playersInside != null && playersInside.Count > 0)
            {
                float totalInputX = playersInside.Sum(x => x.Input.CachedInput.x);
                float totalInputY = playersInside.Sum(x => x.Input.CachedInput.y);
                Vector2 totalInput = new(totalInputX, totalInputY);
                rb2d.AddForce(movementMultiplier * Time.fixedDeltaTime * totalInput, ForceMode2D.Impulse);
            }
        }
    }
}
