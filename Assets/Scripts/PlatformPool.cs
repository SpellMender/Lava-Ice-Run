using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPool : MonoBehaviour
{
    public int PlatformPoolSize = 5;
    public GameObject platformPrefab;
    //public float spawnRate = 1.5f; //deprecated. Now using GameController variable spawnRate
    public float platformMin = 5f;
    public float platformMax = 10f;
    public float startSpawnX = 10f;
    public float ElevOdds = 3f;
    public float ElevHeight = 1f;


    private GameObject[] platforms;
    private Vector2 objectPoolPosition= new Vector2 (-15, -25);
    private float timeSinceLastSpawned;
    private float spawnYPosition = -2.1f; //REVIEW THIS
    private int currentPlatform = 0;
    private bool isStartPlatform = true;

    // Start is called before the first frame update
    void Start()
    {
        platforms = new GameObject[PlatformPoolSize];
        for (int i = 0; i < PlatformPoolSize; i++)
        {
            platforms[i] = (GameObject) Instantiate(platformPrefab, objectPoolPosition, Quaternion.identity);
        }
        timeSinceLastSpawned = GameController.instance.spawnRate; //this code here for immediate spawnage
    }

    // Update is called once per frame
    void Update()
    {
        float spawnYOriginal = spawnYPosition;
        if ((GameController.instance.gameOn) && timeSinceLastSpawned >= GameController.instance.spawnRate) //checks if its time to spawn
        {
            timeSinceLastSpawned = 0; //resets the spawn timer
            float spawnXPosition = Random.Range(platformMin, platformMax); //Declares a random distance between platforms
            if (Mathf.Floor(Random.Range(1, ElevOdds)) == 1) //change height of 1 in x platforms (where x is ElevOdds)
            {
                spawnYPosition += ElevHeight;
                if (GameController.instance.lastPlatformPosition.y == spawnYPosition) //Raise it again if the last platform was raised
                {
                    spawnYPosition += ElevHeight;
                }
            }

            if (!isStartPlatform) //If its not the starting platform
            {
                //spawns a new platform relative to the last, and stores its location for use later
                platforms[currentPlatform].transform.position = new Vector2(spawnXPosition, spawnYPosition);
                GameController.instance.lastPlatformPosition = platforms[currentPlatform].transform.position;
            }
            else
            {
                platforms[currentPlatform].transform.position = new Vector2(startSpawnX, spawnYPosition); //Starting platform has its own start spawn position
                GameController.instance.lastPlatformPosition = platforms[currentPlatform].transform.position;
                isStartPlatform = false;
            }

            currentPlatform++;
            if(currentPlatform >= PlatformPoolSize) //When we reach the end of the platform pool, use the first.
            {
                currentPlatform = 0;
            }
            spawnYPosition = spawnYOriginal; // reset the height of the next platform to normal
        }

        timeSinceLastSpawned += Time.deltaTime;
    }
}
