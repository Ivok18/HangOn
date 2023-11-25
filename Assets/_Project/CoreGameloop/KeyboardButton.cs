using UnityEngine;
using UnityEngine.UI;

namespace HangOn.Gameloop
{
    public class KeyboardButton : MonoBehaviour
    {
        [SerializeField] private char letter;

       

        public char Letter => letter;

    }
}