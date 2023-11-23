using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HangOn.Gameloop
{
    public class HangmanContainer : MonoBehaviour
    {
        [SerializeField] private GameObject[] hangmanStages;

        public GameObject[] Stages => hangmanStages;
    }
}