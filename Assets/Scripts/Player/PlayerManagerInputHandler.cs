using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
    public class PlayerManagerInputHandler : MonoBehaviour
    {
        public event Action<PlayerController> OnPlayerJoin;
        public event Action OnPlayerExit;

        [SerializeField]
        private PlayerInputManager playerInputManager;

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

        [SerializeField]
        private UiPlayerDisplay uiPrefab;

        [SerializeField]
        private Transform uiParent;

        [SerializeField]
        private Color[] colorForEachPlayerIndex;

        private List<UiPlayerDisplay> displays = new();

        [UsedImplicitly]
        private void OnPlayerJoined(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerJoined);
            CreateSpawnParticlesAt(playerInput.transform);
            SpawnPlayerUi(playerInput.playerIndex);
            var playerController = playerInput.GetComponent<PlayerController>();
            playerController.OnPlayerStart += OnPlayerStart;
			playerController.SetPlayerColor(colorForEachPlayerIndex[playerInput.playerIndex]);
            OnPlayerJoin?.Invoke(playerController);
        }

        private void OnPlayerStart()
        {
            playerInputManager.enabled = false;
            playerInputManager.DisableJoining();
        }

        [UsedImplicitly]
        private void OnPlayerLeft(PlayerInput playerInput)
        {
            audioSource.PlayOneShot(playerLeft);
            CreateSpawnParticlesAt(playerInput.transform);
            OnPlayerExit?.Invoke();
        }

        private void SpawnPlayerUi(int index)
        {
            var playerUi = Instantiate(uiPrefab, uiParent);
            displays.Add(playerUi);
            playerUi.Setup($"PLAYER #{index + 1}", colorForEachPlayerIndex[index]);
        }

        private void DestroyPlayerUi(int index)
        {
            var display = displays[index];
            displays.RemoveAt(index);
            Destroy(display.gameObject);
        }

        private void CreateSpawnParticlesAt(Transform parent)
        {
            var particles = Instantiate(playerJoinedParticles, parent);
            particles.transform.localPosition = Vector3.zero;
        }
    }
}
