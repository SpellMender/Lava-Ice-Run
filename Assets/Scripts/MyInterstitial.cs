using UnityEngine;
using UnityEngine.SceneManagement;

public class MyInterstitial : AppBrainSdk.LevelCompleteInterstitial
{
    public static MyInterstitial instance = null;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    protected override void OnNextLevel()
    {
        print("interstitial LOADED");
        AudioScript.instance.addGameCt();
        AudioScript.instance.SetNewGame(); //We set NewGame to true in the object that will remain after the reset.
        SceneManager.LoadScene(0);
    }
}