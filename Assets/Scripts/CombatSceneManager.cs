using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CombatSceneManager : MonoBehaviour
{
    public static CombatSceneManager Instance;

    public GameObject CombatantContainer;
    private GameObject AllyContainer;
    private GameObject EnemyContainer;

    public GameObject[] Characters;

    public CharacterManager CurrentlySelectedCharacter;

    public SelectedIndicatorManager Selector;

    public int TurnIndex { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            CombatGridManager.Instance.ClearGridVisuals();
            ProgressTurn();
        }
    }

    private void UpdateTurnVisuals()
    {
        Selector.transform.position = Characters[TurnIndex].transform.position;
    }

    public void ProgressTurn()
    {
        if (CurrentlySelectedCharacter != null)
        {
            if (CurrentlySelectedCharacter.State != CharacterManager.CharacterState.Dead)
            {
                CurrentlySelectedCharacter.State = CharacterManager.CharacterState.Idle;
            }
        }
        TurnIndex++;
        TurnIndex = TurnIndex > Characters.Length - 1 ? 0 : TurnIndex;
        UpdateTurnVisuals();
        CurrentlySelectedCharacter = GetActiveCharacter();
        CombatCameraManager.Instance.SetCameraTarget(Characters[TurnIndex]);
    }

    public CharacterManager GetActiveCharacter()
    {
        return Characters[TurnIndex].GetComponent<CharacterManager>();
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
