using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour,IInteractable
{
    public bool HasBeenCollected = false;
    public GameObject Popup;
    int GoldIndex = -1;
    public void Interact()
    {
        if (HasBeenCollected == false)
        {
            HasBeenCollected = true;
            SaveLoadManager.instance._GameData.LastGoldCollectedTime = new CustomTime(DateTime.Now);
            print("ATTEMPTING TO INCREASE GOLD by using index " + GoldIndex);
            if (GoldIndex != -1)
            {
                SaveLoadManager.instance._GameData.ItemData[0].Quantity++;
                print("INCREASING GOLD");
            }
            Popup.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Popup.SetActive(!HasBeenCollected);
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        yield return new WaitForSeconds(0.1f);
        GoldIndex = SaveLoadManager.instance._GameData.FindItemIndex("Gold");
        CheckIfCanCollect();
    }

    public void CheckIfCanCollect()
    {
        DateTime Time1 = DateTime.Parse(CustomTime.ReturnParseableTime(SaveLoadManager.instance._GameData.SaveTime));
        DateTime Time2 = DateTime.Parse(CustomTime.ReturnParseableTime(SaveLoadManager.instance._GameData.LastGoldCollectedTime));
        if (Time1 == null || Time2 == null)
            return;
        if(Time1.Day == Time2.Day)
        {
            Popup.SetActive(Time1.Day != Time2.Day);
            HasBeenCollected = Time1.Day == Time2.Day;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
