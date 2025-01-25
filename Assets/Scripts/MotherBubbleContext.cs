using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherBubbleContext : MonoBehaviour
    {
        [SerializeField]
        private BubbleWeightSystem bubbleWeightSystem;

        public BubbleWeightSystem BubbleWeightSystem => bubbleWeightSystem;
    }
}
