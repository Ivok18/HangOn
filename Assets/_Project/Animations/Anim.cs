using UnityEngine;

namespace HangOn.Animations
{
    public class Anim : MonoBehaviour
    {
        public string ReferenceName;
        public int Id;

        public delegate void PlayAnimCallback(int id);
        public static event PlayAnimCallback OnPlay;

        public void Play(int id)
        {
            OnPlay?.Invoke(id);
        }

    }
}
