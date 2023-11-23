using HangOn.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Gameloop
{
    public class HangmanManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hangmanStages;
        [SerializeField] private KeyboardButton[] keyboardButtons;
        [SerializeField] private GameObject letterContainer;
        [SerializeField] private GameObject wordContainer;
        [SerializeField] private int correctGuesses;
        [SerializeField] private int incorrectGuesses;
        [SerializeField] TextAsset possibleWord;
        [SerializeField] private string word;
        [SerializeField] private int currStageIndex;
        [SerializeField] private int score;
        [SerializeField] private int letterBonus;
        [SerializeField] private int wordBonus;
        private int lastStageIndex = 11;

        public delegate void IncorrectGuessCallback(int currStageIndex);
        public static event IncorrectGuessCallback OnIncorrectGuess;

        public delegate void ResetHangmanCallback(int currStageIndex);
        public static event ResetHangmanCallback OnResetHangman;

        public delegate void ScoreChangedCallback(int newScore);
        public static event ScoreChangedCallback OnScoreChanged;
      

        public GameObject[] Stages => hangmanStages;
        public GameObject LetterContainer => letterContainer;
        public GameObject WordContainer => wordContainer;

        private void Awake()
        {
            foreach(var keyboardButton in keyboardButtons)
            {
                Button button = keyboardButton.GetComponent<Button>();
                button.onClick.AddListener(delegate { CheckLetter(keyboardButton.Letter.ToString()); });
            }
        }

        private void Start()
        {
            NewWord();   
        }

        public void NewWord()
        {
            foreach (Transform child in wordContainer.GetComponentInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }
            do
            {
                word = GenerateWord().ToUpper();
            } while (word.Length > 7);

            int order = -1;
            foreach (var letter in word)
            {
                order++;
                var temp = Instantiate(LetterContainer, wordContainer.transform);
                LetterContainer letterContainer = temp.GetComponent<LetterContainer>();
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                if (isFirstLetter || isLastLetter)
                {
                    letterContainer.BlackUnderscore();
                    letterContainer.ShowLetter(letter.ToString());
                }
                else
                {
                    letterContainer.HideLetter(letter.ToString());
                }
            }
        }
        public string GenerateWord()
        {
            string[] wordList = possibleWord.text.Split("\n");
            string line = wordList[Random.Range(0, wordList.Length - 1)];
            return line.Substring(0, line.Length - 1);
        }

        public void CheckLetter(string inputLetter)
        {
            bool isLetterInWord = false;
            for(int i = 0; i < word.Length; i++)
            {
                if (inputLetter == word[i].ToString())
                {
                    isLetterInWord = true;
                    correctGuesses++;
                    GainLetterPoints();
                    TextMeshProUGUI[] lettersInWord = WordContainer.GetComponentsInChildren<TextMeshProUGUI>();
                    lettersInWord[i].text = inputLetter;
                }     
            }
            if (!isLetterInWord)
            {
                incorrectGuesses++;
                NextStage();
                bool hasRunEnded = currStageIndex > lastStageIndex;
                if (!hasRunEnded)
                {
                    OnIncorrectGuess?.Invoke(currStageIndex);
                }               
            }
            CheckOutcome(currStageIndex);
        }

        public void CheckOutcome(int currentStageIndex)
        {
            bool hasRunEnded = currentStageIndex > lastStageIndex;
            bool hasFoundWord = correctGuesses == word.Length - 2;
            if (hasRunEnded)
            {
                // reset stage index to first stage index
                currStageIndex = 0;
                UIEndOfRun.Open();
            }
            if(hasFoundWord)
            {
                GainWordPoints();
            }

        }

        public void ResetKeyboard()
        {
            foreach(var keyboardButton in keyboardButtons)
            {
                keyboardButton.GetComponent<Button>().interactable = true;
            }
        }

        public void ResetHangman()
        {
            OnResetHangman?.Invoke(currStageIndex);
        }

        public void ResetScore()
        {
            score = 0;
        }

        public void ResetCorrectGuesses()
        {
            correctGuesses = 0;
        }

        public void ResetIncorrectGuesses()
        {
            incorrectGuesses = 0;
        }

        public void ResetGame()
        {
            ResetKeyboard();
            ResetHangman();
            ResetScore();
            ResetCorrectGuesses();
            ResetIncorrectGuesses();
            OnScoreChanged?.Invoke(0);
        }

        public void SetStage(int stage)
        {
            currStageIndex = stage - 1;
        }

        public void GainLetterPoints()
        {
            score += letterBonus;
            OnScoreChanged?.Invoke(score);
        }

        public void GainWordPoints()
        {
            score += wordBonus;
            OnScoreChanged?.Invoke(score);
        }

        public void NextStage()
        {
            currStageIndex++;
        }
    }
}
