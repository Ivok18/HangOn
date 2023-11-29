using HangOn.Gameloop;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HangOn.Leaderboard
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance;
        [SerializeField] private List<int> scores;
        [SerializeField] private int firstScore;
        [SerializeField] private int secondScore;
        [SerializeField] private int thirdScore;

        public int First => firstScore;
        public int Second => secondScore;
        public int Third => thirdScore;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            HangmanManager.OnRunEnded += OnRunEnded;
        }

        private void OnDisable()
        {
            HangmanManager.OnRunEnded -= OnRunEnded;
        }

        private void Start()
        {
            scores.Add(0);
            scores.Add(0);
            scores.Add(0);
        }

        private void OnRunEnded(int score)
        {
            Debug.Log("refresh 2");
            scores.Add(score);
            scores.Distinct();
            if(scores.Count < 3)
            {
                scores.AddRange(Enumerable.Repeat(-1, 3 - scores.Count));
            }
            scores.Sort((a, b) => b.CompareTo(a));           
            firstScore = scores[0];
            secondScore = scores[1];
            thirdScore = scores[2];
        }

       

    }
}