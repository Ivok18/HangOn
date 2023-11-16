using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HangOn.ToolDebug
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private List<UIDebugButton> buttons;
        public static ButtonManager Instance;
        [HideInInspector]
        [Tooltip("Button id to show/hide")]
        public int ButtonId;


        private void Awake()
        {
            Instance = this;
        }

        public UIDebugButton GetButtonByID(int id)
        {
            var button = buttons.Where(x => x.Id == id).FirstOrDefault();

            if(button == null)
            {
                Debug.Log($"invalid button while trying to get button with id '{id}'");
                return null;
            }

            return button;
        }

        public UIDebugButton GetLeftScrollButton()
        {
            var leftScroll = buttons.Where(x => x.ScrollDirection == SCROLL_DIRECTION.SCROLL_LEFT).FirstOrDefault();
            return leftScroll;
        }

        public UIDebugButton GetRightScrollButton()
        {
            var rightScroll = buttons.Where(x => x.ScrollDirection == SCROLL_DIRECTION.SCROLL_RIGHT).FirstOrDefault();
            return rightScroll;
        }
       
        public void Activate(int id)
        {
            var uiDebugButton = buttons.Where(x => x.Id == id).FirstOrDefault();

            if (uiDebugButton == null)
            {
                Debug.Log($"invalid id while trying to activate button with id '{id}'");
                return;
            }

            uiDebugButton.Activate();
        }

        public void Disable(int id)
        {
            var uiDebugButton = buttons.Where(x => x.Id == id).FirstOrDefault();

            if (uiDebugButton == null)
            {
                Debug.Log($"invalid id while trying to disabke button with id '{id}'");
                return;
            }

            uiDebugButton.Disable();
        }

    }
}