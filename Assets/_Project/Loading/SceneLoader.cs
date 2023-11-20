using UnityEngine;
using UnityEngine.SceneManagement;

namespace HangOn.LoadingScene
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
