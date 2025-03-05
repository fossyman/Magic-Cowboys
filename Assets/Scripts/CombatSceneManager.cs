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

    public CharacterManager[] Characters;

    public CharacterManager CurrentlySelectedCharacter;

    public SelectedIndicatorManager Selector;

    public List<GameObject> TargetingInstances = new List<GameObject>();
    public List<CharacterManager> CurrentlyTargetedCharacters = new List<CharacterManager>();

    public int TurnIndex { get; private set; } = 0;

    public LayerMask layerMask;


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
        Characters = new CharacterManager[AllyContainer.transform.childCount + EnemyContainer.transform.childCount];
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

            if (Input.GetMouseButtonDown(0) && CurrentlySelectedCharacter && CurrentlySelectedCharacter.State == CharacterManager.CharacterState.Moving)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);

                if (hasHit)
                {
                    Vector3 Gridsquare;
                    float Normal = Vector3.Angle(Vector3.up, hit.normal);
                    print(Normal * Mathf.Deg2Rad);
                    if (Normal > 0)
                    {
                        Gridsquare = new Vector3(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y) + 0.5f, Mathf.FloorToInt(hit.point.z));
                    }
                    else
                    {
                        Gridsquare = new Vector3(Mathf.FloorToInt(hit.point.x), hit.point.y, Mathf.FloorToInt(hit.point.z));
                    }

                    switch (CurrentlySelectedCharacter.State)
                    {
                        case CharacterManager.CharacterState.Idle:
                            CombatGridManager.Instance.ClearGridVisuals();
                            break;
                        case CharacterManager.CharacterState.Moving:
                            CurrentlySelectedCharacter.AttemptMoveToNewPoint(Gridsquare);
                            CombatGridManager.Instance.ClearGridVisuals();
                            break;
                        case CharacterManager.CharacterState.Attacking:
                            CombatGridManager.Instance.ClearGridVisuals();
                            break;
                        case CharacterManager.CharacterState.Dead:
                            CombatGridManager.Instance.ClearGridVisuals();
                            break;
                    }
                }
            }
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
        CombatCameraManager.Instance.SetCameraTarget(Characters[TurnIndex].gameObject);
        CombatUIManager.Instance.UpdateAvailableAbilities(Characters[TurnIndex].CharacterData.Abilities);
    }

    private void UpdateTurnVisuals()
    {
        Selector.transform.position = Characters[TurnIndex].transform.position;
    }

    public CharacterManager GetActiveCharacter()
    {
        return Characters[TurnIndex].GetComponent<CharacterManager>();
    }

    public List<CharacterManager> GetTargetableCharacters(Vector3 Origin, int Range)
    {
        List<CharacterManager> FoundCharacters = new List<CharacterManager>();
        Vector3 OriginGridPos = CombatGridManager.Instance.CalculateGridSquare(Origin);
        for (int i = 0; i < Characters.Length; i++)
        {
            Vector3 CharacterPos = CombatGridManager.Instance.CalculateGridSquare(Characters[i].transform.position);
            if (Characters[i] != CurrentlySelectedCharacter && Vector2.Distance(new Vector2(OriginGridPos.x, OriginGridPos.z), new Vector2(CharacterPos.x, CharacterPos.z)) <= Range)
            {
                FoundCharacters.Add(Characters[i]);
            }
        }
        return FoundCharacters;
    }

    void BuildCharacterArray()
    {
        CharacterManager[] FoundCharacters = new CharacterManager[Characters.Length];
        print(FoundCharacters.Length);

        int LastIndex = 0;

        for (int i = 0; i < AllyContainer.transform.childCount; i++)
        {
            print("FOUND " + AllyContainer.transform.GetChild(i).gameObject.name);
            FoundCharacters[LastIndex] = (AllyContainer.transform.GetChild(i).GetComponent<CharacterManager>());
            LastIndex++;
        }

        for (int y = 0; y < EnemyContainer.transform.childCount; y++)
        {
            print("FOUND " + EnemyContainer.transform.GetChild(y).gameObject.name);
            print("ON INDEX " + LastIndex);
            FoundCharacters[LastIndex] = (EnemyContainer.transform.GetChild(y).GetComponent<CharacterManager>());
            LastIndex++;
        }
        Characters = FoundCharacters;
    }



    public void ClearTargetingInstances()
    {
        for (int i = 0; i < TargetingInstances.Count; i++)
        {
            Destroy(TargetingInstances[i]);
        }

        TargetingInstances.Clear();
    }
}
