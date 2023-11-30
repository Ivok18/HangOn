using DG.Tweening;
using HangOn.Animations;
using UnityEngine;

public class MainMenu_Credits_Anim1_WebsiteButton : MonoBehaviour
{
    private static int id;
    [SerializeField] private Transform animTarget;
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float growDuration;
    [SerializeField] private float shrinkDuration;

    public delegate void PlayWebsiteButtonAnimRequestCallback(int id);
    public static event PlayWebsiteButtonAnimRequestCallback OnRequestPlayWebsiteButtonAnim;

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
        Play();
    }

    private void SetupAnimAndStart(Transform animTarget)
    {
        // 
        Sequence subSequence = DOTween.Sequence();
        Sequence sequence = DOTween.Sequence();


        // grow  
        //float maxScale = 0.175f;
        //float growDuration = 0.55f;
        Tween tween1 = animTarget.DOScale(maxScale, growDuration).SetEase(Ease.Linear);
        subSequence.Append(tween1);

        // shrink 
        //float minScale = 0.15f;
        //float shrinkDuration = 0.55f;
        Tween tween2 = animTarget.DOScale(minScale, shrinkDuration).SetEase(Ease.Linear);
        subSequence.Append(tween2);

        // infinite loop
        sequence.Append(subSequence).SetLoops(-1, LoopType.Yoyo);
    }

    public static void Play()
    {
        OnRequestPlayWebsiteButtonAnim?.Invoke(id);
    }

    private void OnPlay(int animId)
    {
        if (id != animId)
            return;

        //Whatever the animation needs to start playing
        SetupAnimAndStart(animTarget);
    }
}
