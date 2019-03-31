using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StepCounter : MonoBehaviour
{
    TextMeshProUGUI text;
    public TextMeshProUGUI gameOverText;

    int StepCount { get; set; } = 0;

    void Start()
    {
        Player.Player.OnAddStepCount += CountPlus;
        InputHandler.OnRestart += CountZero;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void CountZero()
    {
        StepCount = 0;
        text.text = "Steps: " + StepCount;
        gameOverText.text = "Game Over \nSteps: " + StepCount;
    }

    private void CountPlus()
    {
        StepCount++;
        text.text = "Steps: " + StepCount;
        gameOverText.text = "Game Over \nSteps: " + StepCount;
    }
}
