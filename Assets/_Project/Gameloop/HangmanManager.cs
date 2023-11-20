using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HangOn.Gameloop
{
    public class HangmanManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] hangmanStages;
        [SerializeField] private GameObject letterContainer;
        [SerializeField] private GameObject wordContainer;
        [SerializeField] private int incorrectGuesses;
        [SerializeField] TextAsset possibleWord;
        [SerializeField] private string word;
      

        public GameObject[] Stages => hangmanStages;
        public GameObject LetterContainer => letterContainer;
        public GameObject WordContainer => wordContainer;

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
                    letterContainer.DisplayOpaqueLetter(letter.ToString());
                }
                else
                {
                    letterContainer.DisplayTransparentLetter(letter.ToString());
                }
            }
        }
        public string GenerateWord()
        {
            string[] wordList = possibleWord.text.Split("\n");
            string line = wordList[Random.Range(0, wordList.Length - 1)];
            return line.Substring(0, line.Length - 1);
        }
    }
}
