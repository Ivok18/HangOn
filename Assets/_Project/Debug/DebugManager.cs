using UnityEngine;
using UnityEngine.SceneManagement;
using HangOn.Navigation;

namespace HangOn.ToolDebug
{
    public class DebugManager : MonoBehaviour
    {
        private int idOfLastClickedButton;
        public SCROLL_TYPE currScrollType;
        public SCROLL_DIRECTION currScrollDir;

  
        private void OnEnable()
        {
            UIDebugButton.OnClick += PerformResponse;
            SceneManager.sceneLoaded += SetupOnSceneLoad;
            UIWindowManager.OnWindowOpen += SyncOnWindowOpen;
            UIWindowManager.OnWindowClose += SyncOnWindowClose;
        }

        private void OnDisable()
        {
            UIDebugButton.OnClick -= PerformResponse;
            SceneManager.sceneLoaded -= SetupOnSceneLoad;
            UIWindowManager.OnWindowOpen -= SyncOnWindowOpen;
            UIWindowManager.OnWindowClose -= SyncOnWindowClose;
        }

        private void Start()
        {
            currScrollType = SCROLL_TYPE.SCENE_SCROLL;
        }
        public void PerformResponse(int buttonID)
        {
            idOfLastClickedButton = buttonID;
            UIDebugButton button = ButtonManager.Instance.GetButtonByID(buttonID);
            bool shouldSwitchScrollType = button.ScrollType != SCROLL_TYPE.NONE ? true : false;
            bool shouldScroll = button.ScrollDirection != SCROLL_DIRECTION.NONE ? true : false;

            if (shouldSwitchScrollType)
            {
                SwitchScrollType();             
            }           
            else if(shouldScroll)
            {
                bool shouldScrollLeft = button.ScrollDirection == SCROLL_DIRECTION.SCROLL_LEFT;
                bool shouldScrollRight = button.ScrollDirection == SCROLL_DIRECTION.SCROLL_RIGHT;
                if (shouldScrollLeft)
                {
                    currScrollDir = SCROLL_DIRECTION.SCROLL_LEFT;
                }
                else if(shouldScrollRight)
                {
                    currScrollDir = SCROLL_DIRECTION.SCROLL_RIGHT;
                }         
                Scroll();
            }             
        }

      
        public void SwitchScrollType()
        {
            int activeSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            UIDebugButton lastPressedButton = ButtonManager.Instance.GetButtonByID(idOfLastClickedButton);

            bool isScrollTypeScene = currScrollType == SCROLL_TYPE.SCENE_SCROLL;
            bool isScrollTypeUI = currScrollType == SCROLL_TYPE.UI_SCROLL;
            bool isFirstScene = activeSceneBuildIndex == 0;
            bool isSceneButtonPressed = lastPressedButton.ScrollType == SCROLL_TYPE.SCENE_SCROLL;
            bool isUIButtonPressed = lastPressedButton.ScrollType == SCROLL_TYPE.UI_SCROLL;

            if (isSceneButtonPressed)
            {
                // Avoid reloading the first scene when we're already in it
                // .. and we're in SCENE SCROLL mode
                if (isFirstScene && isScrollTypeScene)
                {
                    Debug.Log("Avoid reloading the first scene when we're already in it, and we're in SCENE SCROLL mode");
                    return;
                }

                // Switch to SCENE SCROLL and go back to first scene when goat button is pressed
                currScrollType = SCROLL_TYPE.SCENE_SCROLL;
                SceneManager.LoadScene(0);
                UIWindowManager.Instance.CloseAll();
                Debug.Log("Switch to SCENE SCROLL and go back to first scene when goat button is pressed");
            }
            if (isUIButtonPressed)
            {
                // Avoid reloading first ui window when we're on another ui window
                if (isScrollTypeUI)
                {
                    Debug.Log("Avoid reloading first ui window when we're on another ui window");
                    return;
                }

                // Switch to UI_SCROLL
                currScrollType = SCROLL_TYPE.UI_SCROLL;
                UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                ButtonManager.Instance.Disable(leftScrollButton.Id);
                UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                ButtonManager.Instance.Activate(rightScrollButton.Id);
                UIWindow firstUIWindow = UIWindowContainer.Instance.First();
                UIWindowManager.Instance.Open(firstUIWindow);
                Debug.Log("Switch to UI SCROLL SCROLL ");
            }
        }

        public void Scroll()
        {
            bool isCurrScrollTypeSceneScroll = currScrollType == SCROLL_TYPE.SCENE_SCROLL;
            bool isCurrScrollTypeUIScroll = currScrollType == SCROLL_TYPE.UI_SCROLL;
            bool hasPressedLeftScrollButton = currScrollDir == SCROLL_DIRECTION.SCROLL_LEFT;
            bool hasPressedRightScrollButton = currScrollDir == SCROLL_DIRECTION.SCROLL_RIGHT;

            if (hasPressedLeftScrollButton || hasPressedRightScrollButton)
            {
                // Scroll left / right in SCENE SCROLL
                if (isCurrScrollTypeSceneScroll)
                {
                    Debug.Log("Scroll right / left in SCENE SCROLL");
                    UIDebugButton buttonPressed = ButtonManager.Instance.GetButtonByID(idOfLastClickedButton);
                    int targetSceneBuildIndex = buttonPressed.TargetSceneBuildIndex;
                    SceneManager.LoadScene(targetSceneBuildIndex);
                }
                // Scroll left / right in UI SCROLL
                else if (isCurrScrollTypeUIScroll)
                {
                    // Scroll left
                    if (hasPressedLeftScrollButton)
                    {
                        Debug.Log("Scroll left in UI SCROLL");
                        UIWindowManager.Instance.CloseAll();
                        UIWindow previousUIWindow = UIWindowContainer.Instance.Previous();
                        if (previousUIWindow == null)
                        {
                            UpdateScrollButtons();
                            return;
                        }
                           
                        UIWindowManager.Instance.Open(previousUIWindow);
                        UpdateScrollButtons();                
                    }

                    // Scroll right
                    else if (hasPressedRightScrollButton)
                    {
                        Debug.Log("Scroll right in UI SCROLL");
                        UIWindowManager.Instance.CloseAll();
                        UIWindow nextUIWindow = UIWindowContainer.Instance.Next();
                        if (nextUIWindow == null)
                        {
                            UpdateScrollButtons();
                            return;
                        }

                        UIWindowManager.Instance.Open(nextUIWindow);
                        UpdateScrollButtons();
                    }
                }
            }
        }

        public void SetupOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            currScrollType = SCROLL_TYPE.SCENE_SCROLL;
            UpdateScrollButtons();
        }

        public void SyncOnWindowOpen(UIWindow window)
        {
            currScrollType = SCROLL_TYPE.UI_SCROLL;
            UpdateScrollButtons();
        }

        public void SyncOnWindowClose()
        {
            bool isAnyOpen = IsAnyWindowOpen() == true ? true : false;

            if(isAnyOpen)
            {
                UpdateScrollButtons();
                return;
            }
                
            currScrollType = SCROLL_TYPE.SCENE_SCROLL;
            UpdateScrollButtons();
        }

        public void UpdateScrollButtons()
        {     
            bool isSceneScroll = currScrollType == SCROLL_TYPE.SCENE_SCROLL;
            bool isUIScroll = currScrollType == SCROLL_TYPE.UI_SCROLL;

            if(isSceneScroll)
            {     
                int currBuildIndex = SceneManager.GetActiveScene().buildIndex;
                int sceneCount = SceneManager.sceneCountInBuildSettings;
                bool shouldActivateBoth = currBuildIndex > 0 && currBuildIndex < sceneCount - 1;
                bool shouldActivateLeftOnly = currBuildIndex == sceneCount - 1;
                bool shouldActivateRightOnly = currBuildIndex == 0;

                if (shouldActivateBoth)
                {
                    Debug.Log("both");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Activate(leftScrollButton.Id);
                    ButtonManager.Instance.Activate(rightScrollButton.Id);
                }
                else if (shouldActivateLeftOnly)
                {
                    Debug.Log("left only");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Activate(leftScrollButton.Id);
                    ButtonManager.Instance.Disable(rightScrollButton.Id);
                }
                else if (shouldActivateRightOnly)
                {
                    Debug.Log("right only");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Disable(leftScrollButton.Id);
                    ButtonManager.Instance.Activate(rightScrollButton.Id);
                }
            }
            if(isUIScroll)
            {
                int uiWindowCount = UIWindowContainer.Instance.WindowCount;
                int currUIWindowIndex = UIWindowContainer.Instance.GetCurrentIndex();

                bool shouldActivateBoth = currUIWindowIndex > 0 && currUIWindowIndex < uiWindowCount - 1;
                bool shouldActivateLeftOnly = currUIWindowIndex == uiWindowCount - 1;
                bool shouldActivateRightOnly = currUIWindowIndex == 0;

                if (shouldActivateBoth)
                {
                    Debug.Log("both");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Activate(leftScrollButton.Id);
                    ButtonManager.Instance.Activate(rightScrollButton.Id);
                }
                else if (shouldActivateLeftOnly)
                {
                    Debug.Log("left only");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Activate(leftScrollButton.Id);
                    ButtonManager.Instance.Disable(rightScrollButton.Id);
                }
                else if (shouldActivateRightOnly)
                {
                    Debug.Log("right only");
                    UIDebugButton leftScrollButton = ButtonManager.Instance.GetLeftScrollButton();
                    UIDebugButton rightScrollButton = ButtonManager.Instance.GetRightScrollButton();
                    ButtonManager.Instance.Disable(leftScrollButton.Id);
                    ButtonManager.Instance.Activate(rightScrollButton.Id);
                }
            }

        }

        public bool IsAnyWindowOpen()
        {
            if(UIWindowManager.Instance.CanFindOpenWindow())
            {
                return true;
            }
            return false;
        }
    }
}
