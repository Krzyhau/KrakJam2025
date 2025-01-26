using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherBubbleMeshStretcherTester : MonoBehaviour
    {
        [SerializeField] private MotherBubbleShapeManipulator motherBubbleShapeManipulator;
        [SerializeField] private float targetSize;
        void Start() 
        {
            motherBubbleShapeManipulator.SetStretcherState(transform, true);
        }

        private void Update()
        {
            motherBubbleShapeManipulator.TargetSize = targetSize;
        }
    }
}
