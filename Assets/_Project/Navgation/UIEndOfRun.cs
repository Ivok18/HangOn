using HangOn.Gameloop;
using HangOn.Leaderboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Navigation
{
    public class UIEndOfRun : MonoBehaviour
    {

        [Header("Leaderboard Settings")]
        [SerializeField] private Image highscorePlaceholder;
        [SerializeField] private Sprite firstScore;
        [SerializeField] private Color firstScoreColor;
        [SerializeField] private Sprite secondScore;
        [SerializeField] private Color secondScoreColor;
        [SerializeField] private Sprite thirdScore;
        [SerializeField] private Color thirdScoreColor;


        [Header("Other")]
        [SerializeField] private HangmanManager hangmanManager;
        [SerializeField] private TextMeshProUGUI scoreText;
        public static int id;
        private UIWindow window;
        private bool shouldRefreshUI;
        private bool hasRefreshedUI;

      
        public delegate void OpenUIEndOfRunRequest(int uiWindowId);
        public static event OpenUIEndOfRunRequest OnOpenUIEndOfRunRequest;

       

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
            window = GetComponent<UIWindow>();
        }


        public void Update()
        {
            bool isWindowOpen = window.IsOpen == true ? true : false;
            if (!isWindowOpen)
            {
                shouldRefreshUI = false;
                hasRefreshedUI = false;
                return;
            }

            shouldRefreshUI = true;
            if (shouldRefreshUI && !hasRefreshedUI)
            {
                RefreshOnce();
            }
        }

        public void RefreshUI()
        {
            Debug.Log("refresh 1");
            int score = hangmanManager.Score;
            if (score == LeaderboardManager.Instance.First)
            {
                // Display the first score sprite
                highscorePlaceholder.gameObject.SetActive(true);
                highscorePlaceholder.sprite = firstScore;
                highscorePlaceholder.color = firstScoreColor;
                Game_EndOfRun_Anim1_GoldMedal.Play();
            }
            else if (score == LeaderboardManager.Instance.Second)
            {
                // Display the second score sprite
                highscorePlaceholder.gameObject.SetActive(true);
                highscorePlaceholder.sprite = secondScore;
                highscorePlaceholder.color = secondScoreColor;
                Game_EndOfRun_Anim1_SilverMedal.Play();
            }
            else if (score == LeaderboardManager.Instance.Third)
            {
                // Display the third score sprite
                highscorePlaceholder.gameObject.SetActive(true);
                highscorePlaceholder.sprite = thirdScore;
                highscorePlaceholder.color = thirdScoreColor;
                Game_EndOfRun_Anim1_BronzeMedal.Play();
            }
            else
            {
                // Disable placeholder
                highscorePlaceholder.gameObject.SetActive(false);
            }
            scoreText.text = score.ToString();
        }

        public void RefreshOnce()
        {
            RefreshUI();
            shouldRefreshUI = false;
            hasRefreshedUI = true;
        }

        public static void Open()
        {
            OnOpenUIEndOfRunRequest?.Invoke(id);
        }
    }
}