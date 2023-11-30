using DG.Tweening;
using HangOn.Animations;
using HangOn.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UntangleGoats.Audio;

namespace HangOn.Gameloop
{
    public class HangmanManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hangmanStages;
        [SerializeField] private KeyboardButton[] keyboardButtons;
        [SerializeField] private GameObject letterContainer;
        [SerializeField] private GameObject wordContainer;
        [SerializeField] private int nbOfCorrectGuess;
        [SerializeField] private int nbOfCorrectGuessLeft;
        [SerializeField] private int incorrectGuesses;
        [SerializeField] TextAsset possibleWord;
        [SerializeField] private string word;
        [SerializeField] private int currStageIndex;
        [SerializeField] private int score;
        [SerializeField] private int letterBonus;
        [SerializeField] private int wordBonus;
        [SerializeField] private List<string> lettersFound;
        [SerializeField] private List<string> lettersNotFound;
        [SerializeField] private AudioClip correctLetterSfx;
        [SerializeField] private AudioClip correctWordSfx;
        [SerializeField] private AudioClip incorrectLetterSfx;
        private int lastStageIndex = 11;
        HashSet<string> generatedWords = new HashSet<string>(); // Keep track of generated words

        public delegate void IncorrectGuessCallback(int currStageIndex);
        public static event IncorrectGuessCallback OnIncorrectGuess;

        public delegate void ResetHangmanCallback(int currStageIndex);
        public static event ResetHangmanCallback OnResetHangman;

        public delegate void ScoreChangedCallback(int newScore);
        public static event ScoreChangedCallback OnScoreChanged;

        public delegate void RunEndedCallback(int finalScore);
        public static event RunEndedCallback OnRunEnded;

        public delegate void GuessedLetterCallback(LetterContainer letterContainer);
        public static event GuessedLetterCallback OnLetterGuessed;

        public delegate void GuessedWordCallback();
        public static event GuessedWordCallback OnWordGuessed;

        public GameObject[] Stages => hangmanStages;
        public GameObject LetterContainer => letterContainer;
        public GameObject WordContainer => wordContainer;

        public int Score => score;
        
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
                Random.InitState((int)System.DateTime.Now.Ticks);
                word = GenerateWord().ToUpper();
            } while (word.Length > 7 || generatedWords.Contains(word));

            // Add the newly generated word to the set
            generatedWords.Add(word);

            nbOfCorrectGuessLeft = word.Length;
            var firstLetter = word[0];
            var lastLetter = word[word.Length - 1];
            if (firstLetter == lastLetter)
            {
                // Handle the case where the first and last letters are the same
                Debug.Log("First and last letters are the same.");
            }
            //bool firstIsLast = word[0] == word[word.Length - 1];
            int order = -1;
            foreach (var letter in word)
            {
                order++;
                CreateLetterContainer(letter, order);        
            }
        }

        private void CreateLetterContainer(char letter,int order)
        {
            var temp = Instantiate(LetterContainer, wordContainer.transform);
            LetterContainer letterContainer = temp.GetComponent<LetterContainer>();
            letterContainer.SetAttachedLetter(letter.ToString());

            bool isFirstLetter = order == 0; ;
            bool isLastLetter = order == word.Length - 1;
            bool isDuplicate = word.IndexOf(letter) != word.LastIndexOf(letter);
            if (isFirstLetter || isLastLetter)
            {
                letterContainer.ShowLetter(letter.ToString());
                letterContainer.DisableCross();
                letterContainer.BlackUnderscore();
                TryDisableInKeyboard(letter.ToString());
                nbOfCorrectGuessLeft--;           
            }
            else
            {
                
                if(isDuplicate && (word[0] == letter || word[word.Length - 1] == letter))
                {
                    letterContainer.ShowLetter(letter.ToString());
                    nbOfCorrectGuessLeft--;
                    Debug.Log($"Letter {letter} is a duplicate of either the first letter o");
                }
                else
                {
                    letterContainer.HideLetter(letter.ToString());
                    Debug.Log($"Letter {letter} is not a duplicate.");
                }                
            }
        }

        public string GenerateWord()
        {
            string[] wordList = possibleWord.text.Split("\n");
            wordList = wordList.Distinct().ToArray();
            Debug.Log(wordList.Length);
            string line = wordList[Random.Range(0, wordList.Length)];          
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
                    nbOfCorrectGuessLeft--;
                    GainLetterPoints();
                    AddToLettersFound(inputLetter);
                    TextMeshProUGUI[] lettersInWord = WordContainer.GetComponentsInChildren<TextMeshProUGUI>();
                    lettersInWord[i].text = inputLetter;
                    var letterContainer = WordContainer.GetComponentsInChildren<LetterContainer>().Where(x => x.AttachedLetter == inputLetter).FirstOrDefault();
                    
                    OnLetterGuessed?.Invoke(letterContainer);
                }     
            }
            if (!isLetterInWord)
            {
                incorrectGuesses++;
                NextStage();
                bool hasRunEnded = currStageIndex > lastStageIndex;
                if (!hasRunEnded)
                {
                    SoundManager.Instance.PlaySound(incorrectLetterSfx);
                    OnIncorrectGuess?.Invoke(currStageIndex);
                }               
            }
            CheckOutcome(currStageIndex);
        }

        public void CheckOutcome(int currentStageIndex)
        {
            bool hasRunEnded = currentStageIndex > lastStageIndex;
            bool hasFoundWord = nbOfCorrectGuessLeft <= 0;
            if (hasRunEnded)
            {
                // reset stage index to first stage index
                currStageIndex = 0;
                OnRunEnded?.Invoke(score);
                UIEndOfRun.Open();
             
            }
            if(hasFoundWord)
            {
                GainWordPoints();
                StartCoroutine(NewWord(1f));
                Reset_Word();
                OnWordGuessed?.Invoke();

                Game_Anim1_FindWord.Play();
            }
        }

        //RUNTIME DEBUG
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
                Random.InitState((int)System.DateTime.Now.Ticks);
                word = GenerateWord().ToUpper();
            } while (word.Length > 7 || generatedWords.Contains(word));

            // Add the newly generated word to the set
            generatedWords.Add(word);

            nbOfCorrectGuessLeft = word.Length;
            var firstLetter = word[0];
            var lastLetter = word[word.Length - 1];
            if (firstLetter == lastLetter)
            {
                // Handle the case where the first and last letters are the same
                Debug.Log("First and last letters are the same.");
            }
            //bool firstIsLast = word[0] == word[word.Length - 1];
            int order = -1;
            foreach (var letter in word)
            {
                order++;
                CreateLetterContainer(letter, order);
            }
        }

        /*private void ShowLetterIncludingClones(char letter)
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
                nbOfCorrectGuessLeft--;
            }
        }*/

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
            nbOfCorrectGuess = 0;
        }

        public void ResetIncorrectGuesses()
        {
            incorrectGuesses = 0;
        }

        public void ResetStage()
        {
            SetStage(1);
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
            ResetHangman();
            ResetKeyboard();
            ResetStage();
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
            if(nbOfCorrectGuessLeft > 0)
            {
                SoundManager.Instance.PlaySound(correctLetterSfx);
            } 
            OnScoreChanged?.Invoke(score);
        }

        public void GainWordPoints()
        {
            score += wordBonus;
            SoundManager.Instance.PlaySound(correctWordSfx);
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
            if (nbOfCorrectGuess >= word.Length  - 2)
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
                nbOfCorrectGuess++;
                lettersNotFound.Remove(letterContainer.AttachedLetter);
            }
        }
    }
}
