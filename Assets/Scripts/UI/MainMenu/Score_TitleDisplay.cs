using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AG
{
    public class Score_TitleDisplay : MonoBehaviour
    {
        [Header("UI_Text (Drops).")]
        public TMP_Text scoreText;

        private void Start()
        {
            UpdateHighScoreText();
        }

        void UpdateHighScoreText()
        {
            scoreText.text = PlayerPrefs.GetFloat(Score_InGameDisplay.HighScoreKey, 0).ToString("0");
        }
    }
}