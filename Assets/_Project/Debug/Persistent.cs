using UnityEngine;

namespace HangOn.ToolDebug
{
    public class Persistent : MonoBehaviour
    {
        private static Persistent instance;

        void Awake()    
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(transform.gameObject);
            }
           
        }
    }
}
