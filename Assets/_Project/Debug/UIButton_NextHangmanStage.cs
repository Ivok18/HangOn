using HangOn.Gameloop;
using UnityEngine;

namespace HangOn.ToolDebug
{
    public class UIButton_NextHangmanStage : MonoBehaviour
    {
        [SerializeField] private HangmanManager hangmanManager;
        [SerializeField] private int currStageIndex;

        public GameObject[] Stages => hangmanManager.Stages;
       

        public void Next()
        {
            currStageIndex++;

            bool isHangmanFull = currStageIndex > Stages.Length - 1;
            if(!isHangmanFull)
            {
                // enable current stage
                Stages[currStageIndex].SetActive(true);

                // disable stage before current stage if it exists
                bool previousStageExist = currStageIndex - 1 >= 0;
                if (previousStageExist)
                {
                    Stages[currStageIndex - 1].SetActive(false);
                }
            }
            else
            {
                // disable current stage (which is the last stage)
                Stages[Stages.Length - 1].SetActive(false);

                // reset stage index to first stage index
                currStageIndex = 0;

                // enable first stage
                Stages[currStageIndex].SetActive(true);
            }
           
          
            

           
            /*if (incorrectGuesses >= maxIncorrectGuessesAllowed)
            {
                incorrectGuesses = maxIncorrectGuessesAllowed;
                hangmanStages[incorrectGuesses].SetActive(false);
                incorrectGuesses = 0;
                hangmanStages[0].SetActive(true);
            }

            if(incorrectGuesses == 0)
            {
                hangmanStages[0].SetActive(false);
                hangmanStages[1].SetActive(true);
                incorrectGuesses = 1;
                return;
            }

            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
            if (incorrectGuesses - 2 < 0)
                return;

            hangmanStages[incorrectGuesses - 2].SetActive(false);   */
        }
    }
}
