using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Used a reference to figure out what I'd need to use for sound but arranged things in the methods myself.
// [RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    public GameManager gameManager;
    Rigidbody rigidbody;
    
    Vector3 home;
    GameState lastState;



    // sound stuff
    public AudioClip audioClip;
    public AudioSource audioData;
    float volumeSpeed;

    public float maxSpeed = 3.7f;
    public AnimationCurve volumeCurve;
    public AnimationCurve pitchCurve;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        home = transform.position;
        try
        {
            audioData = GetComponent<AudioSource>();
            audioData.clip = audioClip;
            rigidbody = GetComponent<Rigidbody>();
        }
        catch { Debug.LogError("Ball Load Error"); }
        
    }

    void Update()
    {
        if( !gameManager.getEditState() && gameManager.getGameState() == GameState.Playing ) // locks the ball in place while editing, releases it to the physics engine when running the map
        {
            rigidbody.isKinematic = false;
        }
        else
        {
            rigidbody.isKinematic = true;
        }

        checkOutOfBounds();
        checkEditState();
        lastState = gameManager.getGameState();

        /* 
        if ( Input.GetKey(KeyCode.Space) ) // test code for releasing the ball
        {
            rigidbody.isKinematic = false;
        }
        */
    }
    private void OnCollisionStay(Collision collision) // handles playing the rolling sound effect with varying volume while rolling on terrain
    {
        
        if (audioData && collision.gameObject.CompareTag("Terrain"))
        {
            // Debug.Log("" + audioData.isPlaying + volumeSpeed);
            volumeSpeed = rigidbody.velocity.magnitude;
            if (audioData.isPlaying == false && volumeSpeed >= 0.1f)
            {
                Debug.Log("ball play");
                audioData.Play();
                audioData.volume = Mathf.Clamp01(volumeSpeed / 20);
            }
            else if (audioData.isPlaying == true && volumeSpeed < 0.1f)
            {
                // Debug.Log("ball pause");
                audioData.Pause();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (audioData && audioData.isPlaying == true && collision.gameObject.CompareTag("Terrain"))
        {
            audioData.Pause();
        }
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void checkOutOfBounds() // resets the ball if it falls out of bounds
    {
        if (transform.position.y < -10)
        {
            gameManager.startEdit();
            resetBall();
        }
    }

    void checkEditState() // makes sure the ball is in the starting position while editing
    {
        if (gameManager.getEditState())
        {
            resetBall();
        }
        if (lastState == GameState.Playing && gameManager.getGameState() != GameState.Playing)
        {
            resetBall();
        }
    }
    public void resetBall() // returns the ball to its starting position and locks it in place
    {
        transform.position = home;
        rigidbody.isKinematic = true;
    }
}
