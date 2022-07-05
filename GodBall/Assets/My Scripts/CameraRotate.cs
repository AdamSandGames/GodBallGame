using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float cameraDistance;
    public float camKeyV;
    public float dragV;
    public float scrollSpeed;
    public float pointTransSpeed;

    float verticalSens = 0.5f;
    Vector3 lookTarget;

    public GameManager gameManager; // background music pass in
    public AudioSource auSource;
    public AudioClip bgMusic;
    private bool hasPlayed;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lookTarget = new Vector3(0,5,0);
        cameraDistance = transform.position.magnitude;

        // Speed set
        camKeyV = 150f;
        dragV = 600f;
        scrollSpeed = 130f;
        pointTransSpeed = 50f;

        hasPlayed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.getGameState() == GameState.Start)
        {
            transform.RotateAround(lookTarget, new Vector3(0, 1, 0), camKeyV * 0.3f * Time.deltaTime);
        }
        keyboardInputs();
        mouseInputs();
        distanceManager();
        transform.LookAt(lookTarget);
        if(!hasPlayed && gameManager.getGameState() == GameState.Playing)
        {
            startMusic();
        }
    }

    void startMusic()
    {
        hasPlayed = true;
        auSource.clip = bgMusic;
        auSource.Play();
    }

    void keyboardInputs()
    {
        if ( Input.GetKey("left") || Input.GetKey(KeyCode.Z) )
        {
            transform.RotateAround(lookTarget, new Vector3(0,1,0), camKeyV * -1 * Time.deltaTime);
        }
        if ( Input.GetKey("right") || Input.GetKey(KeyCode.X) )
        {
            transform.RotateAround(lookTarget, new Vector3(0, 1, 0), camKeyV * 1 * Time.deltaTime);
        }

        // Translate Look Point
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(1, 0, 0) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(1, 0, 0) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(-1, 0, 0) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(-1, 0, 0) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(0, 0, 1) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(0, 0, 1) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0, 0, -1) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(0, 0, -1) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.R))
        {
            transform.position += new Vector3(0, 1, 0) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(0, 1, 0) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.position += new Vector3(0, -1, 0) * pointTransSpeed * Time.deltaTime;
            lookTarget += new Vector3(0, -1, 0) * pointTransSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            lookTarget = new Vector3(0, 0, 0);
        }
        
    }
    void mouseInputs()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Translate( new Vector3( -Input.GetAxis("Mouse X") * dragV * Time.deltaTime, -Input.GetAxis("Mouse Y") * dragV * verticalSens * Time.deltaTime, 0) );
            
        }
        cameraDistance *= 1 + (-Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime);
        if (cameraDistance < 2)
        {
            cameraDistance = 2;
        }
    }
    void distanceManager()
    {
        Vector3 offset = transform.position - lookTarget;
        offset *= 100;
        transform.position = lookTarget + Vector3.ClampMagnitude(offset, cameraDistance);
    }
}
