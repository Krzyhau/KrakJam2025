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
        private GameObject playerJoinedParticles;

        [SerializeField]
        private AudioClip playerLeft;

        [SerializeField]
        private GameObject playerLeftParticles;

        [UsedImplicitly]
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerJoined);
            CreateSpawnParticlesAt(playerInput.transform);
        }

        [UsedImplicitly]
        private void OnPlayerLeft(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerLeft);
            CreateSpawnParticlesAt(playerInput.transform);
        }

        private void CreateSpawnParticlesAt(Transform parent)
        {
            var particles = Instantiate(playerJoinedParticles, parent);
            particles.transform.localPosition = Vector3.zero;
        }
    }
}
