using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFSW.QC;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    private Aezakmi _aezakmi;
    public static GameMenager instance;
    public Vector3 lastLocation;
    int i = 2;

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
        _aezakmi = FindAnyObjectByType<Aezakmi>();
        if (_aezakmi == null)
        {
            Debug.LogError("AnotherScriptOnBall could not find Aezakmi component!");
        }
    }

    [Command("nextLvl")]
    public void NextLevel()
    {
        UnloadLevel();
        var loadedResource = LoadLevel("lvl"+i.ToString());
        i++;
        GameObject CurrentLvl = (GameObject)Instantiate(loadedResource,Vector3.zero,Quaternion.identity);
        if(CurrentLvl == null)
        {
            return;
        }
        Vector3 trn = SpawnPoints["lvl" + i.ToString()];
        _aezakmi.TeleportBall(trn.x, trn.y,trn.z);
    }

    public UnityEngine.Object LoadLevel(string name)
    {
        var levelPrefab = Resources.Load("Assets/Prefabs/courses" + name);//+".prefab"
        if (levelPrefab == null) {
            throw new FileNotFoundException("file: " + name + " not found");
        }
        return levelPrefab;
    }

    public void UnloadLevel()
    {
        GameObject lvl = GameObject.FindWithTag("Level")?.GetComponent<GameObject>();
        if (lvl != null)
        {
            Destroy(lvl);
        }
    }
    
}
