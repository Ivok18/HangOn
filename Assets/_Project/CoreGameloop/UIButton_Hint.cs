using HangOn.Gameloop;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIButton_Hint : MonoBehaviour
{
    private Button button;
    bool shouldForceInteractability;
    bool hasPressedButton;
    [SerializeField] private float waitTime;
    [SerializeField] private HangmanManager hangmanManager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if(shouldForceInteractability && hasPressedButton)
        {
            Debug.Log("force interactable == true");
            StartCoroutine("EnableInteractability");
            shouldForceInteractability = false;
            hasPressedButton = false;
        }
    }


    private void OnEnable()
    {
        HangmanManager.OnRoundReset += OnRoundReset;
    }

    private void OnDisable()
    {
        HangmanManager.OnRoundReset -= OnRoundReset;
    }

    private void OnRoundReset()
    {
        hasPressedButton = true;
        StartCoroutine("EnableInteractability");
    }

    private IEnumerator EnableInteractability()
    {
        yield return new WaitForSeconds(waitTime);
        button.interactable = true;
        shouldForceInteractability = true;
    }
}
