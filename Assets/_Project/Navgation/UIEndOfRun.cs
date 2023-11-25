using HangOn.Gameloop;
using TMPro;
using UnityEngine;

namespace HangOn.Navigation
{
    public class UIEndOfRun : MonoBehaviour
    {
        [SerializeField] private HangmanManager hangmanManager;
        [SerializeField] private TextMeshProUGUI scoreText;
        private bool shouldRefreshUI;
        private bool hasRefreshedUI;

        public static int id;
        private UIWindow window;

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
            int score = hangmanManager.Score;
            scoreText.text = score.ToString() + "P";
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