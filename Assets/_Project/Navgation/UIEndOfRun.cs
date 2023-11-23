using UnityEngine;

namespace HangOn.Navigation
{
    public class UIEndOfRun : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUIEndOfRunRequest(int uiWindowId);
        public static event OpenUIEndOfRunRequest OnOpenUIEndOfRunRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUIEndOfRunRequest?.Invoke(id);
        }
    }
}