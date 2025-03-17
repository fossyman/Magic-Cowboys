using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    public CustomTime InitTime;
    public CustomTime PreviousPlayTime;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        InitTime = new CustomTime(DateTime.Now);
    }

    public void LoadPreviousTime()
    {
        PreviousPlayTime = SaveLoadManager.instance._GameData.SaveTime;
    }
}
