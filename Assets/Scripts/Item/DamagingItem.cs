using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
    [RequireComponent(typeof(Collider2D))]
    public class DamagingItem : MonoBehaviour
    {
        public event Action<BubbleContext> OnSharpTouchedBubble;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out BubbleTrigger bubbleTrigger) && bubbleTrigger.BubbleContext != null)
            {
                OnSharpTouchedBubble?.Invoke(bubbleTrigger.BubbleContext);
            }
        }
    }
}