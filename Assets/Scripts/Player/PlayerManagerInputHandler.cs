using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

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

        [UsedImplicitly]
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerJoined);
            // PARTICLE PLAY
        }

        [UsedImplicitly]
        private void OnPlayerLeft(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerLeft);
            // PARTICLE PLAY
        }
    }
}
