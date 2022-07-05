using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreDisplay : MonoBehaviour
{
    public GameManager gameManager;
    public int score;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        score = gameManager.getScore();
        scoreText.text = "Score: " + score;//.ToString();
    }
}

// score formula
// score = goalval * par / terraindisplacement