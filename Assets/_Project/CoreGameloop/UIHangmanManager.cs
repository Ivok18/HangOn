using UnityEngine;

namespace HangOn.Gameloop
{
    public class UIHangmanManager : MonoBehaviour
    {
        [SerializeField] private HangmanContainer hangmanContainer;

        private void OnEnable()
        {
            HangmanManager.OnIncorrectGuess += OnIncorrectGuess;
            HangmanManager.OnResetHangman += OnResetHangman;
            HangmanManager.OnRewindHangman += OnRewindHangman;

        }
        private void OnDisable()
        {
            HangmanManager.OnIncorrectGuess -= OnIncorrectGuess;
            HangmanManager.OnResetHangman -= OnResetHangman;
            HangmanManager.OnRewindHangman -= OnRewindHangman;
        }
        
        private void OnIncorrectGuess(int currStageIndex)
        {
            bool isHangmanFull = currStageIndex > hangmanContainer.Stages.Length - 1;
            if (!isHangmanFull)
            {
                // enable current stage
                hangmanContainer.Stages[currStageIndex].SetActive(true);

                // disable stage before current stage if it exists
                bool previousStageExist = currStageIndex - 1 >= 0;
                if (previousStageExist)
                {
                    hangmanContainer.Stages[currStageIndex - 1].SetActive(false);
                }
            }
            else
            {
                // disable current stage (which is the last stage)
                hangmanContainer.Stages[hangmanContainer.Stages.Length - 1].SetActive(false);

                // enable first stage
                hangmanContainer.Stages[currStageIndex].SetActive(true);
            }
        }

        private void OnResetHangman(int currStageIndex)
        {
            for(int i = 1; i < hangmanContainer.Stages.Length; i++)
            {
                hangmanContainer.Stages[i].SetActive(false);
            }
            //hangmanContainer.Stages[currStageIndex].SetActive(false);
            hangmanContainer.Stages[0].SetActive(true);
        }

        private void OnRewindHangman(int currStageIndex, int rewindCount)
        {
            if(currStageIndex < 0)
            {
                for (int i = 1; i < hangmanContainer.Stages.Length; i++)
                {
                    hangmanContainer.Stages[i].SetActive(false);
                }
                //hangmanContainer.Stages[currStageIndex].SetActive(false);
                hangmanContainer.Stages[0].SetActive(true);
            }
            else
            {
                for (int i = 0; i < hangmanContainer.Stages.Length; i++)
                {
                    hangmanContainer.Stages[i].SetActive(false);
                }
                hangmanContainer.Stages[currStageIndex].SetActive(true);
            }
            
        }
    }
}