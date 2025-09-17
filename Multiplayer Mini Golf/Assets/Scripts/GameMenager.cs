using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using QFSW.QC;
using QFSW.QC.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    
    private Aezakmi _aezakmi;
    public static GameMenager instance { get; private set; }
    public Vector3 lastLocation;
    int i = 2;

    [SerializeField] private TextMeshProUGUI HolesTxt;
    [SerializeField] private TextMeshProUGUI ParTxt;

    private int holeNum = 1;
    private int[] parNum = new int[4] { 5, 6, 7, 5 };

    Dictionary<string, Vector3> SpawnPoints = new Dictionary<string, Vector3>()
    {
        {"lvl1" , new Vector3(-1.5f, 1, -10.5f) },
        {"lvl2" , new Vector3(5, 1, 5) },
        {"lvl3" , new Vector3(-31, 15, 31)}
    };

    Dictionary<string, int> ParCount = new Dictionary<string, int> // par - expected number of strokes for a hole
    {
        {"lvl1" , 4 },
        {"lvl2" , 9},
        {"lvl3" , 7}
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

    [Command("nextLvl")]
    public void NextLevel()
    {
        _aezakmi = FindAnyObjectByType<Aezakmi>();
        if (_aezakmi == null)
        {
            Debug.LogError("Aezakmi nije nadjen");
        }
        
        string levelToLoadName = "lvl" + i.ToString(); // e.g., "lvl2" if i is 2
        UnloadLevel();
        GameObject levelPrefab = LoadLevel(levelToLoadName);
        if (levelPrefab == null)
        {
            Debug.LogError($"Failed to load prefab for {levelToLoadName}. Aborting NextLevel.");
            return;
        }

        GameObject CurrentLvl = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
        if (CurrentLvl == null)
        {
            Debug.LogError($"Failed to instantiate {levelToLoadName}.");
            return;
        }
        // Ensure CurrentLvl has the "Level" tag if UnloadLevel relies on it
        CurrentLvl.tag = "Level";

        if (SpawnPoints.TryGetValue(levelToLoadName, out Vector3 spawnPosition))
        {
            _aezakmi.TeleportBall(spawnPosition.x, spawnPosition.y, spawnPosition.z);
        }
        else
        {
            Debug.LogError($"Spawn point not found for {levelToLoadName}! Using default or last known.");
            // Optionally, provide a default spawn or handle this error
            _aezakmi.TeleportBall(0, 5, 0); // Example default
        }
        i++;
        if (i > SpawnPoints.Count) // Assuming SpawnPoints has "lvl1", "lvl2", etc.
        {
            i = 1; // Or the starting level index
        }
    }

    public GameObject LoadLevel(string name)
    {
        string resourcePath = "Prefabs/courses/" + name;
        GameObject levelPrefab = Resources.Load<GameObject>(resourcePath);
        if (levelPrefab == null) {
            // More informative error message for Resources.Load
            Debug.LogError("Failed to load level prefab from Resources. Path: " + resourcePath +
                           ". Make sure the prefab exists at 'Assets/Resources/" + resourcePath +
                           ".prefab' and is of type GameObject.");
            throw new FileNotFoundException("Prefab not found in Resources: " + resourcePath);
        }
        return levelPrefab;
    }

    public void UnloadLevel()
    {
        GameObject lvl = GameObject.FindWithTag("Level");
        if (lvl != null)
        {
            Destroy(lvl);//DestroyImmediate
        }
        else
        {
            Debug.LogWarning("UnloadLevel: No GameObject with tag 'Level' found to destroy.");
        }
    }

    public void HoleSound()
    {
        this.GetComponent<AudioSource>().Play();
    }
    
    public void UpdateNavBar()
    {
        Debug.Log("updatujem: " + holeNum.ToString());
        HolesTxt.text = holeNum.ToString();
        ParTxt.text = parNum[holeNum - 1].ToString();
        holeNum++;
    }
}
