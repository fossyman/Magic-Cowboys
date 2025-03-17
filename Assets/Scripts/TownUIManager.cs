using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownUIManager : MonoBehaviour
{
    public static TownUIManager instance;
    public TextMeshProUGUI GoldText;

    public int GoldIndex = -1;
    public bool IsMapOpen = false;
    public GameObject MapScreen;
    public GameObject DesertZoomMapScreen;

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
    }

    void Start()
    {
        StartCoroutine(UpdateTextValues(0.1f));
    }

    IEnumerator UpdateTextValues(float _time = 0.5f)
    {
        yield return new WaitForSeconds(_time);
        GoldIndex = SaveLoadManager.instance._GameData.FindItemIndex("Gold");
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        if (GoldIndex == -1)
        {
            GoldText.text = "0";
            return;
        }

        GoldText.text = SaveLoadManager.instance._GameData.ItemData[GoldIndex].Quantity.ToString("#,#");
    }



    public void MapOpenButton()
    {
        IsMapOpen = !IsMapOpen;

        MapScreen.SetActive(IsMapOpen);
    }

    public void ZoomIntoSection(GameObject _Section)
    {
        DesertZoomMapScreen.SetActive(false);
        _Section.SetActive(true);
    }

    public void CloseAllZoomIns()
    {
        DesertZoomMapScreen.SetActive(false);
    }

    public void OpenCombatScene(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }



}
