using UnityEngine;

namespace HangOn.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (Instance == null)
            {
                Instance = this;
            }
        }


        public void PlayOrPauseMusic()
        {
            if (audioSource.clip == null)
                return;

            if (audioSource.isPlaying)
            {
                PauseMusic();
            }
            else
            {
                PlayMusic();
            }


        }

        public void PlayMusic()
        {
            audioSource.Play();
            Debug.Log("Playing background music..");
        }


        public void PauseMusic()
        {
            audioSource.Pause();
            Debug.Log("Paused background music.. ");
        }


        public bool IsPlaying()
        {
            return audioSource.isPlaying == true ? true : false;
        }

        public void ChangeVolume(float newValue)
        {
            audioSource.volume = newValue;
        }
    }
}