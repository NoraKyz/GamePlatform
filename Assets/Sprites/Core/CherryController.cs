using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class CherryController : MonoBehaviour
{
    public static CherryController Instance;
    public Text cherryDisplayText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public void AddCherry(int amount)
    {
        string saveLocation = Application.persistentDataPath + "/cherry.json";
        if (File.Exists(saveLocation))
        {
            string dataString = File.ReadAllText(saveLocation);
            GameData gameData = JsonUtility.FromJson<GameData>(dataString);
            gameData.cherry += amount;
            dataString = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveLocation, dataString);
        }
        else
        {
            GameData gameData = new GameData();
            gameData.cherry = amount;
            string dataString = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveLocation, dataString);
        }
    }   

    public void TakeCherry(int amount)
    {
        
    }
}
