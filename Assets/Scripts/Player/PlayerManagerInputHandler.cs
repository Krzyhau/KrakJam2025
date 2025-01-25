using UnityEngine;

namespace Monke.KrakJam2025
{
    public class PlayerManagerInputHandler : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip playerJoined;

        [SerializeField]
        private AudioClip playerLeft;

        private void OnPlayerJoined()
        {
            audioSource.PlayOneShot(playerJoined);
            // PARTICLE PLAY
        }

        private void OnPlayerLeft()
        {
            audioSource.PlayOneShot(playerLeft);
            // PARTICLE PLAY
        }
    }
}
