using UnityEngine;

namespace HangOn.Navigation
{
    public class UIPause : MonoBehaviour
    {
        public static int id;

        public delegate void OpenUIPauseRequest(int uiWindowId);
        public static event OpenUIPauseRequest OnOpenUIPauseRequest;

        private void Awake()
        {
            id = GetComponent<UIWindow>().Id;
        }

        public static void Open()
        {
            OnOpenUIPauseRequest?.Invoke(id);
        }
    }
}