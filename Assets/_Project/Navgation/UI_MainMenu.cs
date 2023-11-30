using UnityEngine;
using UnityEngine.SceneManagement;

namespace HangOn.Navigation
{
    public class UI_MainMenu : MonoBehaviour
    {
        public void Play()
        {
            int targetSceneId = 1;
            SceneManager.LoadScene(targetSceneId);
        }

        public void Leaderboard()
        {
            UILeaderboard.Open();
        }   

        public void Settings()
        {
            UISettings.Open();
        }

        public void Credits()
        {
            UICredits.Open();
        }

        public void MoreGames()
        {
            Application.OpenURL("https://manumuudo.itch.io/");
        }
    }
}