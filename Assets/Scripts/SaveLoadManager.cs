using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    public string SaveloadPath = null;
    public string SaveloadName = "Savedata";
    public GameData _GameData;
    public List<Resource> DefaultItemData = new List<Resource>
    {
        new Resource(0,"Gold",0,999999),
        new Resource(1,"Cactus Juice",0,999),
        new Resource(2,"Tumbleweed",0,999),
        new Resource(3,"SnakeOil",0,999),
        new Resource(4,"Sand",0,999),
        new Resource(5,"Coyote Claw",0,999),
        new Resource(6,"Red Flower",0,999),
    };
    private void Awake()
    {
       
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SaveloadPath = Application.persistentDataPath + "/" + SaveloadName + ".json";
    }

    private void Start()
    {
        SaveloadPath = Application.persistentDataPath + "/" + SaveloadName + ".json";
        if (CheckIfSaveExists() == false)
        {
            _GameData = new GameData();
        }
    }

    public bool CheckIfSaveExists()
    {
        return File.Exists(SaveloadPath);
    }

    public void Save()
    {
        _GameData.SaveTime = new CustomTime(DateTime.UtcNow);
        if (_GameData.ItemData.Count <=0 )
        {
            _GameData.ItemData = DefaultItemData;
        }
        string SaveData = JsonUtility.ToJson(_GameData);
        File.WriteAllText(SaveloadPath, SaveData);
    }

    public void Load()
    {
        if (CheckIfSaveExists())
        {
            string LoadedGameData = File.ReadAllText(SaveloadPath);
            _GameData = JsonUtility.FromJson<GameData>(LoadedGameData);
            Debug.Log("Game Loaded Successfully");
        }
        else
        {
            Debug.LogError("!!!Save file failed to load as it doesn't exist!!");
        }
    }

    public void DeleteSave()
    {
        if (CheckIfSaveExists())
        {
            File.Delete(SaveloadPath);
            Debug.Log("Save file deleted successfully");
        }
        else
        {
            Debug.Log("Save file doesn't exist. Cancelling deletion");
        }
    }
}

[Serializable]
public class GameData
{
    //TIME RELATED
    [SerializeField]public CustomTime SaveTime;
    [SerializeField] public CustomTime LastGoldCollectedTime;
    //ITEM & INVENTORY INFORMATION
    [SerializeField] public List<Resource> ItemData = new List<Resource>
    {
        new Resource(0,"Gold",0,999999),
        new Resource(1,"Cactus Juice",0,999),
        new Resource(2,"Tumbleweed",0,999),
        new Resource(3,"SnakeOil",0,999),
        new Resource(4,"Sand",0,999),
        new Resource(5,"Coyote Claw",0,999),
        new Resource(6,"Red Flower",0,999),
    };

    //PARTY & EQUIPMENT

    //PROGRESSION

    public int FindItemIndex(string _name)
    {
        for (int i = 0; i < ItemData.Count; i++)
        {
            if (ItemData[i].Name == _name)
            {
                return i;
            }
        }
        return -1;
    }
}

[Serializable]
public class Resource
{
    [SerializeField]public int Id;
    [SerializeField] public int Quantity;
    [SerializeField] public int MaxQuantity;
    [SerializeField] public string Name;
    public Resource(int id, string name, int quantity, int maxQuantity)
    {
        Id = id;
        Quantity = quantity;
        MaxQuantity = maxQuantity;
        Name = name;
    }
}

[Serializable]
public class CustomTime
{
    public int Second;
    public int Minute;
    public int Hour;
    public int Day;
    public int Month;
    public int Year;
    public CustomTime(DateTime date)
    {
        Second = date.Second;
        Minute = date.Minute;
        Hour = date.Hour;
        Day = date.Day;
        Month = date.Month;
        Year = date.Year;
    }

    public void WhatTime()
    {
        Debug.Log("Second: " + Second + "| Minute: " + Minute + "| Hour: " + Hour + "| Day: " + Day + "| Month: " + Month + "| Year: " + Year);
    }

    public static string ReturnParseableTime(CustomTime _Time)
    {
        return (_Time.Day+"/"+_Time.Month+"/"+_Time.Year+" "+_Time.Hour+":"+_Time.Minute+":"+_Time.Second).ToString();
    }
}