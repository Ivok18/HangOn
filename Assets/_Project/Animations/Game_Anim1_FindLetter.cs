using DG.Tweening;
using HangOn.Gameloop;
using UnityEngine;

namespace HangOn.Animations
{
    public class Game_Anim1_FindLetter : MonoBehaviour
    {
        private static int id;
        [SerializeField] private Transform animTarget;
        [SerializeField] private GameObject startOfPath;
        [SerializeField] private float verticalDistance;
        [SerializeField] private float animDuration;    
        private bool shouldDestroy;

        public delegate void PlayFindLetterAnimRequestCallback(int id);
        public static event PlayFindLetterAnimRequestCallback OnRequestPlayFindLetterAnim;

        private void Awake()
        {
            id = GetComponent<Anim>().Id;
        }

        private void OnEnable()
        {
            Anim.OnPlay += OnPlay;
            HangmanManager.OnLetterGuessed += OnLetterGuessed;

        }

        private void OnDisable()
        {
            Anim.OnPlay -= OnPlay;
            HangmanManager.OnLetterGuessed -= OnLetterGuessed;
        }

        void Start()
        {
            //Play();
        }


        private void Update()
        {
            if (shouldDestroy)
            {
                Destroy(animTarget.gameObject);
                shouldDestroy = false;
            }
        }

        private void OnLetterGuessed(LetterContainer letterContainer)
        {
            animTarget = letterContainer.TextPointsReward.transform;
            animTarget.gameObject.SetActive(true);
            startOfPath = animTarget.gameObject;
            Play();
        }

        private void SetupAnimAndStart(Transform animTarget)
        {
            Tween tween = animTarget.DOMoveY(startOfPath.transform.position.y + verticalDistance, animDuration, true).SetEase(Ease.Linear);
            tween.OnComplete(() => shouldDestroy = true);
        }

        public static void Play()
        {
            OnRequestPlayFindLetterAnim?.Invoke(id);
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