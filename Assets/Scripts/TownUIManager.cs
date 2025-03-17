using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TownUIManager : MonoBehaviour
{
    public static TownUIManager instance;
    public TextMeshProUGUI GoldText;

    public int GoldIndex = -1;
    // Start is called before the first frame update

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
            GoldText.text = "Nah";
            return;
        }

        GoldText.text = SaveLoadManager.instance._GameData.ItemData[GoldIndex].Quantity.ToString("#,#");
    }


}
