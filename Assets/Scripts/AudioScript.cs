using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public static AudioScript instance = null;
    public bool newGame = false;
    public bool closing = false;
    public int gameCt = 1;
    public string message = "new game loaded";

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

    public void SetNewGame()
    {
        newGame = true;
    }
    public void UnsetNewGame()
    {
        newGame = false;
    }
    public void addGameCt()
    {
        gameCt++;
        gameCt = gameCt % 3;
    }
}
