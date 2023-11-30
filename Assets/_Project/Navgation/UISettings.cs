using HangOn.Audio;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Navigation
{
    public class UISettings : MonoBehaviour
    {
        public static int id;
        private UIWindow window;

        public delegate void OpenUISettingsRequest(int uiWindowId);
        public static event OpenUISettingsRequest OnOpenUISettingsRequest;

        [SerializeField] private Slider slider;
        private bool shouldRefreshUI;
        private bool hasRefreshedUI;

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
            AudioSource audioSource = MusicManager.Instance.GetComponent<AudioSource>();
            slider.value = audioSource.volume;
        }

        public void RefreshOnce()
        {
            RefreshUI();
            shouldRefreshUI = false;
            hasRefreshedUI = true;
        }
        public static void Open()
        {
            OnOpenUISettingsRequest?.Invoke(id);
        }
    }
}
