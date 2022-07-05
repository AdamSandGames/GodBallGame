using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStartScreen : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void startButtonCall()
    {
        Debug.Log("Start Button Pressed");
        gameManager.StartButton();
    }
    public void settingsButtonCall()
    {
        gameManager.SettingsButton();
    }
    public void quitButtonCall()
    {
        gameManager.QuitButton();
    }
}
