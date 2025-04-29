using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    public static GameMenager instance;
    public Vector3 lastLocation;

    Dictionary<string, Vector3> SpawnPoints = new Dictionary<string, Vector3>()
    {
        {"lvl1" , new Vector3(-1.5f, 3, -10.5f) },
        {"lvl2" , new Vector3(0, 5, 0) },
        {"lvl3" , new Vector3(-31, 15, 31)}
    };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);

    }
}
