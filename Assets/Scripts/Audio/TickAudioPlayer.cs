using Rubin;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class TickAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private float cooldown = 2;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private AudioClip clip;

        private Ticker ticker;

        private void Awake()
        {
            ticker = TickerCreator.CreateNormalTime(cooldown);
        }

        private void Update()
        {
            if (ticker.Push())
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
