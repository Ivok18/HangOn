using UnityEngine;

namespace HangOn.Navigation
{
    public class UIWindow : MonoBehaviour
    {
        public GameObject Content;
        public string ReferenceName;
        public int Id;
        public bool IsOpen;

        public void Open()
        {
            if (Content == null)           
                return;
    
            Content.SetActive(true);
            IsOpen = true;
        }

        public void Close()
        {
            if (Content == null)
                return;

            Content.SetActive(false);
            IsOpen = false;
            
        }
    }
}
