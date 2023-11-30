using TMPro;
using UnityEngine;

namespace HangOn.Gameloop
{
    public class UIText_Score : MonoBehaviour
    {
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            HangmanManager.OnScoreChanged += OnScoreChanged;
        }
        private void OnDisable()
        {
            HangmanManager.OnScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int newScore)
        {
            text.text = newScore.ToString();
        }
    }
}