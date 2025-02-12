using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CombatSceneManager : MonoBehaviour
{
    public GameObject CombatantContainer;
    private GameObject AllyContainer;
    private GameObject EnemyContainer;

    public GameObject[] Characters;

    public CharacterManager CurrentlySelectedCharacter;

    public SelectedIndicatorManager SelectedCharacterIndicator;
    // Start is called before the first frame update
    void Start()
    {
        AllyContainer = CombatantContainer.transform.GetChild(0).gameObject;
        EnemyContainer = CombatantContainer.transform.GetChild(1).gameObject;
        Characters = new GameObject[AllyContainer.transform.childCount + EnemyContainer.transform.childCount];
        BuildCharacterArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildCharacterArray()
    {
        GameObject[] FoundCharacters = new GameObject[Characters.Length];
        print(FoundCharacters.Length);

        int LastIndex = 0;

        for (int i = 0; i < AllyContainer.transform.childCount; i++)
        {
            print("FOUND " + AllyContainer.transform.GetChild(i).gameObject.name);
            FoundCharacters[LastIndex] = (AllyContainer.transform.GetChild(i).gameObject);
            LastIndex++;
        }

        for (int y = 0; y < EnemyContainer.transform.childCount; y++)
        {
            print("FOUND " + EnemyContainer.transform.GetChild(y).gameObject.name);
            print("ON INDEX " + LastIndex);
            FoundCharacters[LastIndex] = (EnemyContainer.transform.GetChild(y).gameObject);
            LastIndex++;
        }
        Characters = FoundCharacters;
    }
}
