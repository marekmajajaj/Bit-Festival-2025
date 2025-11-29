using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Restart();
        }
        
    }

    void Restart()
    {
        Debug.Log("Spawn");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

