using UnityEngine;

namespace HangOn.Audio
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;

        public void PlaySound()
        {
            if(audioClip == null)
            {
                Debug.LogError($"error while trying to play {audioClip.name}");
                return;
            }
            SoundManager.Instance.PlaySound(audioClip);
        }
    }
}