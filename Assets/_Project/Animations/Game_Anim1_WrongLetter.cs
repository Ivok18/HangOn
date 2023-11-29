using HangOn.Gameloop;
using UnityEngine;
using DG.Tweening;

namespace HangOn.Animations
{
    public class Game_Anim1_WrongLetter : MonoBehaviour
    {
        private static int id;
        [SerializeField] private Transform animTarget;
        [SerializeField] private float maxScale;
        [SerializeField] private float minScale;
        [SerializeField] private float growDuration;
        [SerializeField] private float shrinkDuration;

        public delegate void PlayWrongLetterAnimRequestCallback(int id);
        public static event PlayWrongLetterAnimRequestCallback OnRequestPlayWrongLetterAnim;

        private void Awake()
        {
            id = GetComponent<Anim>().Id;
        }

        private void OnEnable()
        {
            Anim.OnPlay += OnPlay;
        }

        private void OnDisable()
        {
            Anim.OnPlay -= OnPlay;
            HangmanManager.OnIncorrectGuess -= OnIncorrectGuess;
        }

        void Start()
        {
            //Play();
        }

        private void OnIncorrectGuess(int currStageIndex)
        {
           
        }


        private void SetupAnimAndStart(Transform animTarget)
        {
            // 
            Sequence subSequence = DOTween.Sequence();
            Sequence sequence = DOTween.Sequence();


            // grow  
            //float maxScale = 0.175f;
            //float growDuration = 0.55f;
            Tween tween1 = animTarget.DOScale(maxScale, growDuration).SetEase(Ease.InOutFlash);
            subSequence.Append(tween1);

            // shrink 
            //float minScale = 0.15f;
            //float shrinkDuration = 0.55f;
            Tween tween2 = animTarget.DOScale(minScale, shrinkDuration).SetEase(Ease.InOutFlash);
            subSequence.Append(tween2);

            // infinite loop
            sequence.Append(subSequence).SetLoops(2, LoopType.Yoyo);
        }

        public static void Play()
        {
            OnRequestPlayWrongLetterAnim?.Invoke(id);
        }

        private void OnPlay(int animId)
        {
            if (id != animId)
                return;

            //Whatever the animation needs to start playing
            SetupAnimAndStart(animTarget);
        }
    }
}