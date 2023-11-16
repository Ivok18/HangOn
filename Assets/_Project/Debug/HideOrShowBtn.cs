using UnityEngine;

namespace HangOn.ToolDebug
{
    public class HideOrShowBtn : MonoBehaviour
    {
        [SerializeField] private GameObject debugUIInterface;
        [SerializeField] private bool isActive;
      
        public void HideOrShowDebugUI()
        {
            if (!isActive)
                return;

            bool doesUIToolExist = debugUIInterface != null;
            if (doesUIToolExist)
            {
                bool boolean = debugUIInterface.activeSelf == false ? true : false;
                debugUIInterface.SetActive(boolean);
            } 
        }
    }
}
