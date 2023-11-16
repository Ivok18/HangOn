using System;
using System.Collections.Generic;
using UnityEngine;

namespace HangOn.Navigation
{
    [Serializable]
    public class UIWindowContainer : MonoBehaviour
    {
        public List<UIWindow> Windows;
        public static UIWindowContainer Instance;
        [SerializeField] private int currWindowIndex;
        
       
        public int WindowCount => Windows.Count;

        
        private void Awake()
        {
            Instance = this;
        }

       
        public UIWindow Next()
        {
            bool nextWindowExists = currWindowIndex + 1 <= WindowCount - 1;

            if (!nextWindowExists)
            {
                currWindowIndex = WindowCount - 1;
                return null;
            }

            int nextIndex = currWindowIndex + 1;
            currWindowIndex++;
            return Windows[nextIndex];
        }
        public UIWindow Previous()
        {
            bool previousWindowExists = currWindowIndex - 1 >= 0;
           
            if (!previousWindowExists)
            {
                currWindowIndex = 0;
                return null; 
            }

            int previousIndex = currWindowIndex - 1;
            currWindowIndex--;
            return Windows[previousIndex];
        }
        public UIWindow First()
        {
            return Windows[0];
        }
        public UIWindow Last()
        {
            return Windows[WindowCount - 1];
        }
        public int GetCurrentIndex()
        {
            return currWindowIndex;
        }

        public void SetCurrentWindowIndex(int newIndex)
        {
            currWindowIndex = newIndex;
        }

    }
}