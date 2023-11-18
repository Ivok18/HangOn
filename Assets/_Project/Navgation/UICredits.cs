using UnityEngine;

namespace HangOn.Navigation
{
    public class UICredits : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUICreditsRequest(int uiWindowId);
        public static event OpenUICreditsRequest OnOpenUICreditsRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUICreditsRequest?.Invoke(id);
        }
    }
}
