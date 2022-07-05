using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImodeDisplay : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject terrainEditor;
    public Text modeText;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        //terrainEditor = GameObject.Find("MainCamera").GetComponent<PlayerTerrainEditor>();
    }
    // Update is called once per frame
    void Update()
    {
        if(terrainEditor.GetComponent<PlayerTerrainEditor>().getDeformMode() == PlayerTerrainEditor.DeformMode.RaiseLower)
        {
            modeText.text = "Mode: Raise/Lower";//.ToString();
        }
        if (terrainEditor.GetComponent<PlayerTerrainEditor>().getDeformMode() == PlayerTerrainEditor.DeformMode.Smooth)
        {
            modeText.text = "Mode: Smooth";//.ToString();
        }

    }
}
