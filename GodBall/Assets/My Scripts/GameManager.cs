using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState { Start, Playing, Success, Failure };
public class GameManager : MonoBehaviour
{
    public GameState gameState;     // enumeration of primary game states
    public bool editingLevel;       // whether the player can edit the level or if the physics starts running
    public int GameLevel;           // 
    public int[] ScoreArr;

    public Terrain activeTerrain;
    public GameObject ball;
    public GameObject camera;
    public Canvas editorUI;
    //public Canvas ballUI;
    public Canvas startScreenUI;
    //public Canvas successUI;
    //public Canvas failureUI;

    private List<GameObject> goalsArr;
    private string levelSceneName;
    public Scene mainScene;
    public Scene levelScene;

    public PlayerTerrainEditor editorScript;


    // Start is called before the first frame update
    void Start()
    {
        startScreenUI = GameObject.Find("StartScreenUI").GetComponent<Canvas>();
        mainScene = SceneManager.GetSceneByName("MainScene");
        editorScript = GameObject.Find("Main Camera").GetComponent<PlayerTerrainEditor>();
        gameState = GameState.Start;
        GameLevel = 0;
        ScoreArr = new int[]{0, 0, 0, 0 };
        editingLevel = true;
        loadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (gameState)
        {
            case GameState.Start:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameState = GameState.Playing;
                    nextLevel();
                    startEdit();
                }
                break;
            case GameState.Playing:
                // if (current level not loaded) {load current level};
                if (goalsArr != null)
                {
                    if (ball == null)
                    {
                        Debug.Log("ball null");
                    }
                    if (!activeTerrain)
                    {
                        activeTerrain = FindObjectOfType<Terrain>();
                    }
                    if (activeTerrain)
                    {
                        if (editingLevel) // manage terrain layer for editor
                        {
                            activeTerrain.gameObject.layer = 5;
                        }
                        else
                        {
                            activeTerrain.gameObject.layer = 0;
                        }
                    }
                    if (checkGoals())
                    {
                        // Debug.Log("goals checked true");
                        // returnToEdit();
                        setGameState(GameState.Success);
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (editingLevel)
                            endEdit();
                        else
                            returnToEdit();
                    }
                    if (Input.GetKeyDown(KeyCode.Y)) // resets everything
                    {
                        resetLevel();
                    }
                    //if (Input.GetKeyDown(KeyCode.R)) // resets ball and goals
                    //{
                    //    returnToEdit();
                    //}
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        Debug.Log("Deform Mode Raise/Lower");
                        editorScript.changeDeformMode(PlayerTerrainEditor.DeformMode.RaiseLower);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        Debug.Log("Deform Mode Smooth");
                        editorScript.changeDeformMode(PlayerTerrainEditor.DeformMode.Smooth);
                    }
                    //if (Input.GetKeyDown(KeyCode.Alpha3))
                    //{
                    //    Debug.Log("Deform Mode Flatten");
                    //    editorScript.changeDeformMode(PlayerTerrainEditor.DeformMode.Flatten);
                    //}
                    if (Input.GetKeyDown(KeyCode.Alpha4)) // cheat to next level
                    {
                        nextLevel();
                    }
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        gotoStart();
                    }
                }
                break;
            case GameState.Success:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    nextLevel();
                    gameState = GameState.Playing;
                    returnToEdit();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gotoStart();
                }
                break;
            case GameState.Failure:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    gameState = GameState.Playing;
                    returnToEdit();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gotoStart();
                }
                break;
        }
    }

    void loadLevel()
    {
        if(activeTerrain != null)
        {
            editorScript.resetTerrain();
        }
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(SceneManager.GetSceneAt(i) != mainScene)
            {
                SceneManager.UnloadScene(SceneManager.GetSceneAt(i));
            }
        }

        switch (GameLevel)
        {
            default:
                SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
                levelScene = SceneManager.GetSceneByName("Level1");
                break;
            case 0:
                SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
                levelScene = SceneManager.GetSceneByName("Level1");
                break;
            case 1:
                SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
                levelScene = SceneManager.GetSceneByName("Level1");
                break;
            case 2:
                SceneManager.LoadScene("Level2", LoadSceneMode.Additive);
                levelScene = SceneManager.GetSceneByName("Level2");
                break;
            case 3:
                SceneManager.LoadScene("Level3", LoadSceneMode.Additive);
                levelScene = SceneManager.GetSceneByName("Level3");
                break;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void gotoStart()
    {
        startScreenUI.gameObject.SetActive(true);
        gameState = GameState.Start;
        GameLevel = 0;
        ScoreArr = new int[] { 0, 0, 0, 0 };
        editingLevel = true;
        loadLevel();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("varcleaner");
        levelSceneName = levelScene.name;
        activeTerrain = FindObjectOfType<Terrain>();
        ball = GameObject.FindGameObjectWithTag("Player Ball");
        goalsArr = goalListGetter();
        editorScript.loadTerrainChange();

        //if (ball == null)
        //{
        //    Debug.Log("ball null");
        //}
    }
    
    public void resetLevel()
    {
        camera.GetComponent<PlayerTerrainEditor>().resetTerrain();
        returnToEdit();

    }
    public void returnToEdit()
    {
        ball.GetComponent<Ball>().resetBall();
        resetGoals();
        startEdit();
    }
    public void startEdit()
    {
        editingLevel = true;
    }
    public void endEdit()
    {
        editingLevel = false;
        goalsArr = goalListGetter();
        resetGoals();
    }
    bool checkGoals()
    {
        bool result = false;
        int checkall = 0;
        foreach (GameObject g in goalsArr)
        {
            if (g.GetComponent<Goal>().getMyGoal()) checkall++;
        }
        if (checkall == goalsArr.Count)
        {
            result = true;
        }
        return result;
    }
    void resetGoals()
    {
        foreach (GameObject g in goalsArr)
        {
            g.GetComponent<Goal>().resetGoal();
        }
        ScoreArr[GameLevel] = 0;
    }

    List<GameObject> goalListGetter() // returns a list of goals on the current scene
    {
        List<GameObject> tempList = new List<GameObject>();
        GameObject[] tempArr = GameObject.FindGameObjectsWithTag("Goal");
        foreach(GameObject g in tempArr)
        {
            tempList.Add(g);
        }

        return tempList;
    }
    public void setGameState(GameState gs)
    {
        gameState = gs;
    }
    public GameState getGameState()
    {
        return gameState;
    }
    
    public bool getEditState()
    {
        return editingLevel;
    }
    

    public void nextLevel()
    {
        GameLevel += 1;
        loadLevel();
    }
    public void goToLevel(int l)
    {
        GameLevel = l;
        loadLevel();
    }

    public void addScore(int add)
    {
        ScoreArr[GameLevel] += add;
    }

    public int getScore()
    {
        int total = 0;
        foreach(int i in ScoreArr)
        {
            total += i;
        }
        return total;
    }

    public Terrain getActiveTerrain()
    {
        return activeTerrain;
    }
    private void OnApplicationQuit()
    {
        editorScript.resetTerrain();
    }


    public void StartButton()
    {
        Debug.Log("Start Button Called");
        startScreenUI.gameObject.SetActive(false);
        gameState = GameState.Playing;
        nextLevel();
        startEdit();
    }
    public void SettingsButton()
    {
        Debug.Log("Settings Button Called");
    }
    public void QuitButton()
    {
        Debug.Log("Quit Button Called");
        Application.Quit();
    }
}
