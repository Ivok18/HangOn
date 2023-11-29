using DG.Tweening;
using HangOn.Animations;
using UnityEngine;

public class Game_EndOfRun_Anim1_GoldMedal : MonoBehaviour
{
    private static int id;
    [SerializeField] private Transform animTarget;
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;

    public delegate void PlayGoldMedalAnimCallback(int id);
    public static event PlayGoldMedalAnimCallback OnRequestPlayGoldMedalAnim;

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
    }

    void Start()
    {
        //Play();
    }

    private void SetupAnimAndStart(Transform animTarget)
    {
        // 
        Sequence subSequence = DOTween.Sequence();
        Sequence subSequence2 = DOTween.Sequence();
        Sequence sequence = DOTween.Sequence();


        // grow  
        //float maxScale = 0.175f;
        //float growDuration = 0.55f;
        //Tween tween1 = animTarget.DOScale(maxScale, growDuration).SetEase(Ease.InOutFlash);
        //subSequence.Append(tween1);

        // shrink 
        //float minScale = 0.15f;
        //float shrinkDuration = 0.55f;
        Tween tween2 = animTarget.DOScale(minScale, shrinkDuration).SetEase(Ease.InOutFlash);
        subSequence.Append(tween2);

        // grow  
        float _maxScale = 0.32f;
        float _growDuration = 0.10f;
        Tween tween3 = animTarget.DOScale(_maxScale, _growDuration).SetEase(Ease.InOutFlash);
        subSequence2.Append(tween3);

        // shrink
        float _minScale = 0.29f;
        float _shrinkDuration = 0.18f;
        Tween tween4 = animTarget.DOScale(_minScale, _shrinkDuration).SetEase(Ease.InOutFlash);
        subSequence2.Append(tween4);
        subSequence2.SetLoops(-1, LoopType.Yoyo);

        subSequence.Append(subSequence2);

        // infinite loop
        sequence.Append(subSequence);
    }

    public static void Play()
    {
        OnRequestPlayGoldMedalAnim?.Invoke(id);
    }

    private void OnPlay(int animId)
    {
        if (id != animId)
            return;

        //Whatever the animation needs to start playing
        SetupAnimAndStart(animTarget);
    }
}
