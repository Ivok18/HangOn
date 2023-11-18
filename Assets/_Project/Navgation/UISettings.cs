using UnityEngine;

namespace HangOn.Navigation
{
    public class UISettings : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUISettingsRequest(int uiWindowId);
        public static event OpenUISettingsRequest OnOpenUISettingsRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUISettingsRequest?.Invoke(id);
        }
    }
}
