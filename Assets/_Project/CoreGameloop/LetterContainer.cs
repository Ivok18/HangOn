using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Gameloop
{
    public class LetterContainer : MonoBehaviour
    {
        [Header("Animation Without Animation Manager")]
        [SerializeField] private Transform animTarget;
        [SerializeField] private float maxScale;
        [SerializeField] private float minScale;
        [SerializeField] private float growDuration;
        [SerializeField] private float shrinkDuration;
        bool isVisible;

      
        [Header("Other")]
        [SerializeField] private GameObject pointsRewardContainer;
        [SerializeField] private float transparency;
        [SerializeField] private string attachedLetter;
        private Image underscoreImage;
        private TextMeshProUGUI text;

        public string AttachedLetter => attachedLetter;

        public GameObject PointsRewardContainer => pointsRewardContainer;
        private void OnEnable()
        {
            HangmanManager.OnIncorrectGuess += OnIncorrectGuess;
            HangmanManager.OnLetterGuessed += OnLetterGuessed;
        }
        private void OnDisable()
        {
            HangmanManager.OnIncorrectGuess -= OnIncorrectGuess;
            HangmanManager.OnLetterGuessed -= OnLetterGuessed;
        }

        private void Awake()
        {
            underscoreImage = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnLetterGuessed(LetterContainer letterContainer)
        {
            if (letterContainer.AttachedLetter != attachedLetter)
                return;

            ShowLetter(AttachedLetter);
            isVisible = true;
        }

        private void OnIncorrectGuess(int currStageIndex)
        {
            if (animTarget == null || isVisible)
                return;

        
            animTarget.gameObject.SetActive(true);

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
            tween2.OnComplete(() => animTarget.gameObject.SetActive(false));
            subSequence.Append(tween2);

            // infinite loop
            sequence.Append(subSequence).SetLoops(2, LoopType.Yoyo);
            sequence.Play();

        }

        public void BlackUnderscore()
        {
            underscoreImage.color = Color.black;
        }

        public void ShowLetter(string letter)
        {
            text.text = letter;
            isVisible = true;
        }

        public void DisplayTransparentLetter(string letter)
        {
            text.text = letter;
            text.color = new Color(text.color.r, text.color.g, text.color.b, transparency);
        }

        public void HideLetter(string letter)
        {
            text.text = "";
        }

        public void DisableCross()
        {
            animTarget = null;
        }

        public void SetAttachedLetter(string letter)
        {
            attachedLetter = letter;
        }

        
    }
}
