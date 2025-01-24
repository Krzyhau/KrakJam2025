using UnityEngine;

namespace Monke.KrakJam2025
{
    public class AbsorbTrigger : MonoBehaviour
    {
        [SerializeField]
        private MotherPlayerController mother;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            mother.AddPlayerInside(collision.GetComponent<PlayerController>());
            collision.transform.SetParent(mother.transform, false);
        }
    }
}
