using HangOn.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private List<string> lettersFound;
        [SerializeField] private List<string> lettersNotFound;
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
            StartCoroutine(NewWord(0.1f));
        }

        public IEnumerator NewWord(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            lettersFound.Clear();
            lettersNotFound.Clear();
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
                letterContainer.SetAttachedLetter(letter.ToString());
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                if (isFirstLetter || isLastLetter)
                {
                    letterContainer.BlackUnderscore();
                    ShowLetterIncludingClones(letter);
                    TryDisableInKeyboard(letter.ToString());
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
                    AddToLettersFound(inputLetter);
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
                StartCoroutine(NewWord(1f));
                Reset_Word();
            }
        }

        public void NewWord()
        {
            lettersFound.Clear();
            lettersNotFound.Clear();
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
                letterContainer.SetAttachedLetter(letter.ToString());
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                if (isFirstLetter || isLastLetter)
                {
                    letterContainer.BlackUnderscore();
                    ShowLetterIncludingClones(letter);
                    TryDisableInKeyboard(letter.ToString());                
                    //letterContainer.ShowLetter(letter.ToString());               

                }
                else
                {
                    letterContainer.HideLetter(letter.ToString());
                }
                //Show clones
                
            }       
        }

        private void ShowLetterIncludingClones(char letter)
        {
            List<LetterContainer> letterContainers = new();
            foreach (LetterContainer letterContainer1 in wordContainer.GetComponentsInChildren<LetterContainer>())
            {
                letterContainers.Add(letterContainer1);
            }
            List<LetterContainer> clones = letterContainers.FindAll(x => x.AttachedLetter == letter.ToString());
            foreach (var clone in clones)
            {
                clone.ShowLetter(clone.AttachedLetter);
            }
        }

        public void TryDisableInKeyboard(string letter)
        {
            var keyboardButton = keyboardButtons.Where(x => x.Letter.ToString() == letter).FirstOrDefault();

            if (keyboardButton == null)
                return;

            Debug.Log("letter " + letter + " in inside the keyboard");
            Button button = keyboardButton.GetComponent<Button>();
            button.interactable = false;
            lettersFound.Add(letter);
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

        public void Reset_Game()
        {
            ResetKeyboard();
            ResetHangman();
            ResetScore();
            ResetCorrectGuesses();
            ResetIncorrectGuesses();
            OnScoreChanged?.Invoke(0);
        }

        public void Reset_Word()
        {
            ResetKeyboard();
            ResetHangman();
            ResetIncorrectGuesses();
            ResetCorrectGuesses();
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

        public void AddToLettersFound(string letter)
        {
            lettersFound.Add(letter);
        }

        public void RevealRandomLetter()
        {
            if (correctGuesses >= word.Length  - 2)
                return;
            #region debug+
            /*
             foreach(var letter in word)
            {
                order++;
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                if (isFirstLetter || isLastLetter)
                    continue;

                var _letter = lettersFound.Where(x => x == letter.ToString()).FirstOrDefault();
                if (_letter != null)
                {
                    Debug.Log("letter " + _letter + " has been found");
                    continue;
                }              
            }
            */
            #endregion

            int order = -1; 
            foreach (var letter in word)
            {
                order++;
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                bool isHidden = lettersNotFound.Find(x => x == letter.ToString()) != null;
                bool isFound = lettersFound.Find(x => x == letter.ToString()) == null;


                if (!isFirstLetter && !isLastLetter && isFound && !isHidden)
                {
                    lettersNotFound.Add(letter.ToString());
                }
                
            }

            Debug.Log("letters not found :");
            foreach(var letter in lettersNotFound)
            {
                Debug.Log(letter);
            }

            int randomIndex = Random.Range(0, lettersNotFound.Count);
            var ___letter = lettersNotFound[randomIndex].ToString();

  
            order = -1;
            foreach (Transform child in wordContainer.GetComponentInChildren<Transform>())
            {
                
                order++;
                bool isFirstLetter = order == 0;
                bool isLastLetter = order >= word.Length - 1;
                if (isFirstLetter || isLastLetter)
                    continue;

                LetterContainer letterContainer = child.GetComponent<LetterContainer>();
                if (letterContainer.AttachedLetter != ___letter)
                    continue;

                letterContainer.ShowLetter(letterContainer.AttachedLetter);
                AddToLettersFound(letterContainer.AttachedLetter);
                correctGuesses++;
                lettersNotFound.Remove(letterContainer.AttachedLetter);
            }
        }
    }
}
