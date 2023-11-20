using HangOn.Gameloop;
using UnityEngine;

namespace HangOn.ToolDebug
{
    public class UIButton_NewWord : MonoBehaviour
    {
        [SerializeField] private HangmanManager hangmanManager;
        private string word;


        public void NewWord()
        {
            foreach(Transform child in hangmanManager.WordContainer.GetComponentInChildren<Transform>())
            {
                Destroy(child.gameObject);
            }

            word = hangmanManager.GenerateWord();
            foreach(var letter in word)
            {
                var temp = Instantiate(hangmanManager.LetterContainer, hangmanManager.WordContainer.transform);
            }
        }
    }
}