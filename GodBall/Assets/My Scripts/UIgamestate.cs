using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIgamestate : MonoBehaviour
{
    public GameManager gameManager;
    public GameState state;
    public Text stateText;

    // Update is called once per frame
    void Update()
    {
        state = gameManager.getGameState();
        switch(state)
        {
            case GameState.Failure:
                stateText.text = "State: Failure";
                break;
            case GameState.Playing:
                stateText.text = "State: Playing";
                break;
            case GameState.Success:
                stateText.text = "State: Success";
                break;
            case GameState.Start:
                stateText.text = "State: Start";
                break;
            default:
                stateText.text = "State: err";
                break;
        }
    }
}