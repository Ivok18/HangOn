using UnityEngine;
using UnityEngine.SceneManagement;

namespace HangOn.Navigation
{
    public class UI_Game : MonoBehaviour
    {
        public void Game()
        {
            int targetSceneId = 1;
            SceneManager.LoadScene(targetSceneId);
        }

        public void Pause()
        {
            UIPause.Open();
        }

        public void Leaderboard()
        {
            UIWindowManager.Instance.ShouldUIWindowsOverlap = true;
            UILeaderboard.Open();
        }

        public void Settings()
        {
            UIWindowManager.Instance.ShouldUIWindowsOverlap = true;
            UISettings.Open();
        }

        public void AskConfirmMainMenu()
        {
            UIWindowManager.Instance.ShouldUIWindowsOverlap = true;
            UIAskConfirmMainMenu.Open();
        }

        public void MainMenu()
        {
            int targetSceneId = 0;
            SceneManager.LoadScene(targetSceneId);
        }
    }
}
