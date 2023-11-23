using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Gameloop
{
    public class LetterContainer : MonoBehaviour
    {
        private Image underscoreImage;
        private TextMeshProUGUI text;
        [SerializeField] private float transparency;

  
        private void Awake()
        {
            underscoreImage = GetComponentInChildren<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void BlackUnderscore()
        {
            underscoreImage.color = Color.black;
        }

        public void ShowLetter(string letter)
        {
            text.text = letter;
        }

        public void DisplayTransparentLetter(string letter)
        {
            text.text = letter;
            text.color = new Color(text.color.r, text.color.g, text.color.b, transparency);
        }

        public void HideLetter(string letter)
        {
            text.text = "";
        }
    }
}
