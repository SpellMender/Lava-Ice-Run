using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public float scrollSpeed = -1.0f;
    public float spawnRate = 1.5f;
    public float speedExponent = 1.0f;
    public float scoreDifficultyRatio = 0.00025f;
    public bool gameOn = false;
    public bool isDead = false;
    public Vector2 lastPlatformPosition = Vector2.zero;
    public GameObject tapToPlayText;
    public GameObject gameOverText;
    public AudioSource myAudio;
    public Text scoreText;
    public bool newGame; //for all games after the first. This way you can just play right after reloading.

    private int score = 0;
    
    // Awake is called before Start
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            newGame = AudioScript.instance.newGame; //this object will always survive, so we call its newGame variable value.
            if (newGame) //When we restart a new game we want to start immediately rather than waiting for a click.
            {
                gameOn = true;
                tapToPlayText.SetActive(false);
            }

            //GameObject.DontDestroyOnLoad(gameObject);
        }
        //if (instance == null) {
        //    instance = this;
        //} else if (instance != this)
        //{
        //    Destroy (gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Exit();
        }
        PlayerScored();
        if (!gameOn)
        {
            if (!isDead)
            {
                tapToPlayText.SetActive(true);
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (isDead)
                {
                    AudioScript.instance.StartNewGame(); //when we click we set NewGame to true in the object that will remain after the reset.
                    SceneManager.LoadScene(0);
                }
                gameOn = true;
                tapToPlayText.SetActive(false);
            }
        }
        else
        {
            //@score    0 speedExponent = 1 (normal speed)
            //@score 2000 speedExponent = 2 (twice as fast)
            //@score [infinity] speedExponent = 3 (4x as fast)
            speedExponent = (-1 / ((scoreDifficultyRatio * score) + 0.5f)) + 3;

            if (speedExponent < 1) { speedExponent = 1; }
            if (speedExponent > 3) { speedExponent = 3; }
            spawnRate = 1f + Mathf.Pow(0.5f, speedExponent);
            scrollSpeed = -2.5f * (Mathf.Pow(2f, speedExponent));
        }
    }

    public void PlayerDied()
    {
        tapToPlayText.SetActive(true);
        gameOverText.SetActive(true);
        gameOn = false;
        isDead = true;
    }

    public void PlayerScored()
    {
        //The bird can't score if the game is over.
        if (!gameOn)    
            return;
        //If the game is not over, increase the score...
        score++;
        //...and adjust the score text.
        scoreText.text = "SCORE: " + score.ToString();
    }
    public void Exit()
    {
        Application.Quit();
    }
}
