using DG.Tweening;
using HangOn.Gameloop;
using UnityEngine;

namespace HangOn.Animations
{
    public class Game_Anim1_FindWord : MonoBehaviour
    {
        private static int id;
        [SerializeField] private Transform animTarget;
        [SerializeField] private float timeBetweenShakes;
        [SerializeField] private float shakeAngle;
        [SerializeField] private float maxScale;
        [SerializeField] private float minScale;
        [SerializeField] private float growDuration;
        [SerializeField] private float shrinkDuration;

        public delegate void PlayFindWordAnimRequestCallback(int id);
        public static event PlayFindWordAnimRequestCallback OnRequestPlayFindWordAnim;

        private void Awake()
        {
            id = GetComponent<Anim>().Id;
        }

        private void OnEnable()
        {
            Anim.OnPlay += OnPlay;
            HangmanManager.OnWordGuessed += OnWordGuessed;
        }

        private void OnDisable()
        {
            Anim.OnPlay -= OnPlay;
            HangmanManager.OnWordGuessed -= OnWordGuessed;
        }

        void Start()
        {
            //Play();
        }

        private void OnWordGuessed()
        {
            animTarget.gameObject.SetActive(true);
        }
            
        private void SetupAnimAndStart(Transform animTarget)
        {
            
            // 
            Sequence subSequence = DOTween.Sequence();
            Sequence subsequence2 = DOTween.Sequence();
            Sequence sequence = DOTween.Sequence();
            


            // grow  
            //float maxScale = 0.175f;
            //float growDuration = 0.55f;
            Tween tween1 = animTarget.DOScale(maxScale, growDuration).SetEase(Ease.InOutFlash);
            subSequence.Append(tween1);


            // do not move 
            Tween tween2 = animTarget.DOMove(animTarget.position, timeBetweenShakes);
            //subSequence.Append(tween2);

            // rotation to the left
            Vector3 rotation = new Vector3(0, 0, -shakeAngle);
            float rotationDuration = 0.05f;
            Tween tween3 = animTarget.DORotate(rotation, rotationDuration, RotateMode.Fast).SetEase(Ease.InOutFlash);
            //subSequence.Append(tween3);

            // rotation to the right
            Vector3 rotation2 = new Vector3(0, 0, shakeAngle);
            float rotationDuration2 = 0.05f;
            Tween tween4 = animTarget.DORotate(rotation2, rotationDuration2, RotateMode.Fast).SetEase(Ease.InOutFlash);
            //subSequence.Append(tween4);

            // shake movement looop (tween 2, tween 3, and tween 4)
            subsequence2.Append(tween2);
            subsequence2.Append(tween3);
            subsequence2.Append(tween4);
            subsequence2.SetLoops(4, LoopType.Yoyo);

            subSequence.Append(subsequence2);

            // shrink 
            //float minScale = 0.15f;
            //float shrinkDuration = 0.55f;
            Tween tween5 = animTarget.DOScale(minScale, shrinkDuration).SetEase(Ease.InOutFlash);
            subSequence.Append(tween5);

            sequence.Append(subSequence);
            sequence.OnComplete(() => animTarget.gameObject.SetActive(false)); 
        }

        public static void Play()
        {
            OnRequestPlayFindWordAnim?.Invoke(id);
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