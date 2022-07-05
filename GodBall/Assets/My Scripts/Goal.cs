using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager gameManager;
    private float eTime;
    private float floatspeed;
    private float rotatespeed;
    private float min, max;

    private int goalValue;

    public Material baseMat;
    public Material trueMat;
    
    public bool myGoal;

    // Start is called before the first frame update
    void Start()
    {
        goalValue = 100;
        min = transform.position.y;
        max = min + 5;
        myGoal = false;
        floatspeed = 0.5f;
        rotatespeed = -50f;
        transform.GetComponent<MeshRenderer>().material = baseMat;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if( !gameManager.getEditState() && gameManager.getGameState() == GameState.Playing)
        {
            floatyspinny();
        }
        if (myGoal)
        {
            transform.GetComponent<MeshRenderer>().material = trueMat;
        }
        else
        {
            transform.GetComponent<MeshRenderer>().material = baseMat;
        }
    }

    void OnCollisionEnter(Collision collision) // gain 100 score and set the goal state on collision, doesn't seem to work
    {
        // Debug.Log("Goal Collision");
        if (collision.transform.CompareTag("Player Ball") && !myGoal)
        {
            Debug.Log("Goal hit");
            gameManager.addScore(goalValue);
            myGoal = true;
        }
    }

    private void OnTriggerEnter(Collider trig) // gain 100 score and set the goal state on trigger collision
    {
        // Debug.Log("Goal Triggered");
        if (trig.transform.CompareTag("Player Ball") && !myGoal)
        {
            Debug.Log("Goal trigger hit");
            gameManager.addScore(goalValue);
            myGoal = true;
        }
    }


    void floatyspinny()
    {
        eTime = Time.deltaTime;
        if(transform.position.y > (min + max) / 2)
        {
            floatspeed -= eTime * 40;
        }
        else
        {
            floatspeed += eTime * 40;
        }
        transform.parent.Translate( new Vector3(0, eTime * floatspeed, 0) );
        transform.parent.Rotate(new Vector3( 0, eTime * rotatespeed, 0 ));
        
    }

    public void resetGoal()
    {
        myGoal = false;
    }
    public bool getMyGoal()
    {
        return myGoal;
    }
    
}
