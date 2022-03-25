using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace AG
{
    public class Score_InGameDisplay : MonoBehaviour
    {
        [Header("UI_Text (Drops).")]
        public TMP_Text scoreText;
        public float scoreMulti;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] float _curScore;

        public const string HighScoreKey = "HighScore";

        private void Update()
        {
            _curScore += scoreMulti * Time.deltaTime;
            scoreText.text = _curScore.ToString("0");
        }

        private void OnDestroy()
        {
            float currentHighScore = PlayerPrefs.GetFloat(HighScoreKey, 0);

            if (_curScore > currentHighScore)
            {
                PlayerPrefs.SetFloat(HighScoreKey, _curScore);
            }
        }
    }
}