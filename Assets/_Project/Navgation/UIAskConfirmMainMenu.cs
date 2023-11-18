using UnityEngine;

namespace HangOn.Navigation
{
    public class UIAskConfirmMainMenu : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUIAskConfirmRequest(int uiWindowId);
        public static event OpenUIAskConfirmRequest OnOpenUIAskConfirmRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUIAskConfirmRequest?.Invoke(id);
        }
    }
}
