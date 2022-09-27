using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Mirror;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text scoreText; // probably check for null required?
    private int currentScore = 0;

    public struct ScoreMessage : NetworkMessage
    {
        public int score;
    }

    private void Start()
    {
        if (!NetworkClient.active) { return; }

        NetworkClient.ReplaceHandler<ScoreMessage>(OnScore);
    }

    public void SetScore(int score)
    {
        ScoreMessage msg = new ScoreMessage()
        {
            score = score
        };

        NetworkServer.SendToAll(msg);

    }

    public void OnScore(NetworkConnection conn, ScoreMessage msg)
    {
        currentScore = msg.score + currentScore;
        scoreText.text = "Score: " + $"\n{currentScore}";
        Debug.Log("OnScoreMessage " + currentScore);
    }
}
