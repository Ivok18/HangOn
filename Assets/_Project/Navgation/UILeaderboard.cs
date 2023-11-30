using HangOn.Leaderboard;
using TMPro;
using UnityEngine;

namespace HangOn.Navigation
{
    public class UILeaderboard : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI text1st;
        [SerializeField] private TextMeshProUGUI text2nd;
        [SerializeField] private TextMeshProUGUI text3rd;

        public static int id;
        private UIWindow window;

        private bool shouldRefreshUI;
        private bool hasRefreshedUI;

        public delegate void OpenUILeaderboardRequest(int uiWindowId);
        public static event OpenUILeaderboardRequest OnOpenUILeaderboardRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
            window = GetComponent<UIWindow>();
        }

        private void Update()
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
            Debug.Log(LeaderboardManager.Instance.First);
            Debug.Log(LeaderboardManager.Instance.Second);
            Debug.Log(LeaderboardManager.Instance.Third);
            text1st.text = LeaderboardManager.Instance.First.ToString();
            text2nd.text = LeaderboardManager.Instance.Second.ToString();
            text3rd.text = LeaderboardManager.Instance.Third.ToString();
        }

        public void RefreshOnce()
        {
            RefreshUI();
            shouldRefreshUI = false;
            hasRefreshedUI = true;
        }

        public static void Open()
        {
            OnOpenUILeaderboardRequest?.Invoke(id);
        }
    }

}