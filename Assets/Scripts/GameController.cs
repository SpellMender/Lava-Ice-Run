using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
//using AppBrainSdk;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public float scrollSpeed = -1.0f;   //DEFAULT: -5.0f
    public float spawnRate = 1.5f;      //DEFAULT:  1.5f
    public float speedExponent = 1.0f;  //DEFAULT:  1.0f
    public float scoreDifficultyRatio = 0.00025f; //DEFAULT: 0.00025f
    public bool gameOn = false;
    public bool isDead = false;
    public Vector2 lastPlatformPosition = Vector2.zero;
    public GameObject tapToPlayText;
    public GameObject gameOverText;
    public GameObject exitButton;
    public AudioSource myAudio;
    public Text scoreText; //Text object of the score
    public Text hiScoreText; //Text object of the high score
    public bool newGame; //for all games after the first. This way you can just play right after reloading.
    public bool closing = false;
    public bool closingCheck = false; //literally only exists so we can wait a tick to see if the application is closing

    private int score = 0;
    public int gameCt = 1;
    private BannerView bannerView;

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
            gameCt = AudioScript.instance.gameCt;
            if (newGame) //When we restart a new game we want to start immediately rather than waiting for a click.
            {
                gameOn = true;
                tapToPlayText.SetActive(false);
            }
            else MobileAds.Initialize("ca-app-pub-9178989598116738~8412340662"); //only needs to happen once | in this case, when the game first loads
        }
    }

    private void Start()
    {
        hiScoreText.text = "HI SCORE: " + PlayerPrefs.GetInt("hiScore", 0).ToString(); //the old high score is displayed or zero
        this.RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-9178989598116738/8302968955";

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
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
                tapToPlayText.SetActive(true); //Shows when the game first starts (player is still alive)
            }
            if (closingCheck)
            {
                closingCheck = false;
                if (isDead)
                {
                    StartNewGame();
                }
                gameOn = true;
                tapToPlayText.SetActive(false);
                gameOverText.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0))
            {
                //literally skips a frame to make sure we're not trying to close the app
                if (!closingCheck)
                {
                    print("click received");
                    closingCheck = true;
                    return;
                }
            }
        }
        else
        {
            //WHEN scoreDifficultyRatio = 0.00025 AND Max SpeedExponent = 3
            //@score    0 speedExponent = 1 (normal speed)
            //@score 2000 speedExponent = 2 (twice as fast)
            //@score [infinity] speedExponent = 3 (4x as fast)
            speedExponent = 3/*Max speedExponent*/ - (1 / ((scoreDifficultyRatio * score) + 0.5f)); //scoreDifficultyRatio = 0.5/score WHEN speedExponent = 2 || n = 0.5/t WHEN x = 2

            if (speedExponent < 1) { speedExponent = 1; }
            if (speedExponent > 3) { speedExponent = 3; }

            // THE FORULAS BELOW ARE TO BE LEFT ALONE! THEY ARE TO BALANCE THE GAME TO BE PLAYABLE AT ANY SPEED EXPONENT!
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

        //If the score reaches or overshoots the high score, the high score is the same as the score.
        if (score > PlayerPrefs.GetInt("hiScore"))
        {
            PlayerPrefs.SetInt("hiScore", score); //the score value is moved to the high score local storage
            hiScoreText.text = "HI SCORE: " + PlayerPrefs.GetInt("hiScore").ToString(); //the new high score is displayed
        }
    }
    public void Exit()
    {
        closing = true;
        Application.Quit();
    }
    public void StartNewGame()
    {
        ////every 3 games has an interstitial ad
        //print("gameCt: " + gameCt);
        //if (gameCt == 0 && !newGame)
        //{
        //    print("interstitial load");
        //    MyInterstitial.instance.OnLevelComplete();
        //}

        AudioScript.instance.addGameCt();
        AudioScript.instance.SetNewGame(); //We set NewGame to true in the object that will remain after the reset.
        SceneManager.LoadScene(0);
    }
}
