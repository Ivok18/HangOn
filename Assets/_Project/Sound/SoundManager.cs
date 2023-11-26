using UnityEngine;

namespace UntangleGoats.Audio
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource audioSource;
        public static SoundManager Instance;
        private bool canPlaySounds = true;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (audioSource.isPlaying)
                return;

            audioSource.volume = 1;
        }

        public void PlaySound(AudioClip sound, float volume = 1f)
        {
            if (sound == null)
                return;

            if (!canPlaySounds)
            {
                Debug.Log("Cannot play sounds at the moment..");
                return;
            }

            Debug.Log("Can play sound");
            audioSource.volume = volume;
            audioSource.PlayOneShot(sound);
        }  

        public void EnableSounds()
        {
            Debug.Log("sound effects enabled !");
            canPlaySounds = true;
        }

        public void DisableSounds()
        {
            Debug.Log("sound effects disabled !");
            canPlaySounds = false;
        }
    }
}