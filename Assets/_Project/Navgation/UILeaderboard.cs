using UnityEngine;

namespace HangOn.Navigation
{
    public class UILeaderboard : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUILeaderboardRequest(int uiWindowId);
        public static event OpenUILeaderboardRequest OnOpenUILeaderboardRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUILeaderboardRequest?.Invoke(id);
        }
    }

}