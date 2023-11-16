using UnityEngine;
using UnityEngine.UI;

namespace HangOn.ToolDebug
{
    public class UIDebugButton : MonoBehaviour
    {
        public delegate void OnClickCallback(int id);
        public static event OnClickCallback OnClick;

        public int Id;
        [SerializeField] private Button button;
        public int TargetSceneBuildIndex;
        public SCROLL_TYPE ScrollType;
        public SCROLL_DIRECTION ScrollDirection;
       

        private void Start()
        {
            button.onClick.AddListener(TaskOnClick);
        }

        private void TaskOnClick()
        {
            OnClick?.Invoke(Id);
        }

        public void Activate()
        {
            button.interactable = true;
        }

        public void Disable()
        {
            button.interactable = false;
        }
    }
}