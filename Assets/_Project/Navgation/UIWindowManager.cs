using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HangOn.Navigation
{
    public class UIWindowManager : MonoBehaviour
    {
        public static UIWindowManager Instance;
        [SerializeField] private List<UIWindow> windows;

        [HideInInspector]
        public int Id;

        [HideInInspector]
        public bool ShouldUIWindowsOverlap;

        public int CurrWindowIndex;
        public int WindowCount { get => windows.Count; }


        public delegate void WindowOpenCallback(UIWindow window);
        public static event WindowOpenCallback OnWindowOpen;

        public delegate void WindowCloseCallback();
        public static event WindowCloseCallback OnWindowClose;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            //UIVictory.OnOpenUIVictoryRequest += Open;
            UILeaderboard.OnOpenUILeaderboardRequest += Open;
            UISettings.OnOpenUISettingsRequest += Open;
            UICredits.OnOpenUICreditsRequest += Open;
        }

        private void OnDisable()
        {
            //UIVictory.OnOpenUIVictoryRequest -= Open;
            UILeaderboard.OnOpenUILeaderboardRequest -= Open;
            UISettings.OnOpenUISettingsRequest -= Open;
            UICredits.OnOpenUICreditsRequest -= Open;
        }

        public void Open(string referenceName)
        {
            var window = windows.Where(x => x.ReferenceName == referenceName).FirstOrDefault();

            if(window == null)
            {
                Debug.Log($"invalid reference name while trying to open window '{referenceName}'");
                return;
            }      
            if (!ShouldUIWindowsOverlap)
            {
                CloseAll();
            }
            window.Open();
            CurrWindowIndex = windows.IndexOf(window);
            if (UIWindowContainer.Instance != null)
            {
                UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(window));
            }

            OnWindowOpen?.Invoke(window);
        }
        public void Open(int id)
        {
            var window = windows.Where(x => x.Id == id).FirstOrDefault();

            if (window == null)
            {
                Debug.Log($"invalid id while trying to open window with id '{id}'");
                return;
            }
           
            if (!ShouldUIWindowsOverlap)
            {
                CloseAll();
            }
            window.Open();

            CurrWindowIndex = windows.IndexOf(window);
            if (UIWindowContainer.Instance != null)
            {
                UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(window));
            }
            OnWindowOpen?.Invoke(window);
        }

        public void OpenByIndex(int index)
        {
            var window = windows.Where(x => windows.IndexOf(x) == index).FirstOrDefault();

            if (window == null)
            {
                Debug.Log($"invalid id while trying to open window at index '{index}'");
                return;
            }

            if (!ShouldUIWindowsOverlap)
            {
                CloseAll();
            }
            window.Open();

            CurrWindowIndex = windows.IndexOf(window);
            if (UIWindowContainer.Instance != null)
            {
                UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(window));
            }
            OnWindowOpen?.Invoke(window);
        }

        public void Open(UIWindow uiWindow)
        {
            var window = windows.Where(x => x == uiWindow).FirstOrDefault();

            if (window == null)
            {
                Debug.Log($"invalid window while trying to open window '{uiWindow}'");
                return;
            }
           

            if (!ShouldUIWindowsOverlap)
            {
                CloseAll();
            }

            window.Open();

            CurrWindowIndex = windows.IndexOf(window);
            if (UIWindowContainer.Instance != null)
            {
                UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(window));
            }

            OnWindowOpen?.Invoke(window);
        }


        public void OpenAll()
        {
            ShouldUIWindowsOverlap = true;

            foreach (var window in windows)
            {
                Open(window);          
            }
        }
        

        public void Close(string referenceName)
        {
            var window = windows.Where(x => x.ReferenceName == referenceName).FirstOrDefault();

            if(window == null)
            {
                Debug.Log($"invalid reference name while trying to close window '{referenceName}'");
                return;
            }

            window.Close();

            // We update current window index if there is still a window open
            if (CanFindOpenWindow())
            {
                var openWindow = windows.Where(x => x.IsOpen).LastOrDefault();

                if (openWindow != null)
                {
                    CurrWindowIndex = windows.IndexOf(openWindow);
                    if (UIWindowContainer.Instance != null)
                    {
                        UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(openWindow));
                    }
                }
            }

            OnWindowClose?.Invoke();
           
        }
        public void Close(int id)
        {
            var window = windows.Where(x => x.Id == id).FirstOrDefault();

            if (window == null)
            {
                Debug.Log($"invalid id while trying to close window with id '{id}'");
                return;
            }
            window.Close();

            // We update current window index if there is still a window open
            if (CanFindOpenWindow())
            {
                var openWindow = windows.Where(x => x.IsOpen).LastOrDefault();

                if (openWindow != null)
                {
                    CurrWindowIndex = windows.IndexOf(openWindow);
                    if (UIWindowContainer.Instance != null)
                    {
                        UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(openWindow));
                    }
                }
            }

            OnWindowClose?.Invoke();
           
        }
        public void Close(UIWindow uIWindow)
        {
            var window = windows.Where(x => x == uIWindow).FirstOrDefault();

            if (window == null)
            {
                Debug.Log($"invalid window while trying to close window '{uIWindow}'");
                return;
            }
            window.Close();

            // We update current window index if there is still a window open
            if (CanFindOpenWindow())
            {
                var openWindow = windows.Where(x => x.IsOpen).LastOrDefault();

                if (openWindow != null)
                {
                    CurrWindowIndex = windows.IndexOf(openWindow);
                    if (UIWindowContainer.Instance != null)
                    {
                        UIWindowContainer.Instance.SetCurrentWindowIndex(windows.IndexOf(openWindow));
                    }
                }
            }


            OnWindowClose?.Invoke();
        }



        public void CloseAll()
        {
            // We want to save the index of the current open window before we close all window
            // We do this because each time we close a window, we might change the current window index ..
            int lastOpenIndex;

            lastOpenIndex = CurrWindowIndex;
            if (UIWindowContainer.Instance != null)
            {
                lastOpenIndex = UIWindowContainer.Instance.GetCurrentIndex();
            }

            foreach(var window in windows)
            {
                // This is were we might change the index
                Close(window);
            }

            // We set the current index back to the value it had before we closed all windows
            CurrWindowIndex = lastOpenIndex;
            if (UIWindowContainer.Instance != null)
            {
                UIWindowContainer.Instance.SetCurrentWindowIndex(lastOpenIndex);
            }

            // By default (when all windows are closed), ui windows should not be able to overlap each other
            ShouldUIWindowsOverlap = false;
        }

        public void CloseAllOnRoundStart(int currStageNumber, int currRoundNumber)
        {
            CloseAll();
        }
       
        public bool CanFindOpenWindow()
        {
            var openWindow = windows.Where(x => x.IsOpen).LastOrDefault();

            if(openWindow == null)
            {
                Debug.Log("Cannot find open window");
                return false;
            }

            Debug.Log("Found an open window");
            return true;
        }

    }
}
