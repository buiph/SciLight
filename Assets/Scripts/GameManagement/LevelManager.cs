using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; } // This is a singleton object

    public enum Scenes
    {
        StartGame,
        SciLightScene,
        GameOver,
        PersistenceScene
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartGame()
    {

    }

    public void RestartGame()
    {

    }

    public void GameOver()
    {
        
    }
}
