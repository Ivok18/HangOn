using UnityEngine;
using UnityEngine.SceneManagement;

namespace HangOn.Navigation
{
    public class UI_MainMenu : MonoBehaviour
    {
        public void Play()
        {
            int targetSceneId = 2;
            SceneManager.LoadScene(targetSceneId);
        }


    }
}