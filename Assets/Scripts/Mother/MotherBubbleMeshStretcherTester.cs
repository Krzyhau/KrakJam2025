using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherBubbleMeshStretcherTester : MonoBehaviour
    {
        [SerializeField] private MotherBubbleShapeManipulator motherBubbleShapeManipulator;
        void Start() 
        {
            motherBubbleShapeManipulator.SetStretcherState(transform, true);
        }
    }
}
