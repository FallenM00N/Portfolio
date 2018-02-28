using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_scoreText = null;
    [SerializeField] TextMeshProUGUI m_placementText = null;

    public void UpdateValues()
    {
        int score = PlayerPrefs.GetInt("RecentScore");
        int place = PlayerPrefs.GetInt("Placement");

        m_scoreText.text = "Your Overall Score is:" + score;
        m_placementText.text = "You placed <color=#7AEC97FF>" + place + "</color> on the leaderboard!";
    }
}
