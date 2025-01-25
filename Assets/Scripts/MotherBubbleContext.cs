using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherBubbleContext : MonoBehaviour
    {
        [SerializeField]
        private BubbleWeightSystem bubbleWeightSystem;
        [SerializeField]
        private MotherBubbleShapeManipulator shapeManipulator;

        public BubbleWeightSystem WeightSystem => bubbleWeightSystem;
        public MotherBubbleShapeManipulator ShapeManipulator => shapeManipulator;
    }
}
