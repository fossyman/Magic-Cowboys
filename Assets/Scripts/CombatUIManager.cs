using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public Canvas canvas;
    public Button MoveButton;
    public GameObject AbilityButtonPrefab;
    public RectTransform AbilityArea;
    public SO_AbilityData[] CurrentAbilities;
    public static CombatUIManager Instance;

    public Vector3 AttackMenuClosedPosition;
    public Vector3 AttackMenuOpenPosition;
    public Vector3 TargetAttackMenuPosition;
    public bool AttackMenuOpen;

    public bool IsPressed;
    public RectTransform CharacterPanel;
    public enum MOUSESTATE {idle,dragging}
    public MOUSESTATE CurrentMouseState = MOUSESTATE.idle;
    public Vector3 MouseClickPos;

    public CircleSpinTest CharacterCircle;
    public bool CanRotateCharacters = true;

    private Vector3 ScaledMousePosition;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        MoveButton.onClick.AddListener(ActivateMovement);
        TargetAttackMenuPosition = AttackMenuClosedPosition;
    }

    private void Update()
    {
        ScaledMousePosition = Input.mousePosition / canvas.scaleFactor;
        AbilityArea.localPosition = Vector3.Lerp(AbilityArea.localPosition, TargetAttackMenuPosition,15*Time.deltaTime);
        IsPressed = Input.GetMouseButton(0);
        if (Input.GetMouseButtonDown(0))
        {
            MouseClickPos = ScaledMousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            CanRotateCharacters = true;

        }
        if (IsPressed && MouseClickPos != Input.mousePosition)
        {
            CurrentMouseState = MOUSESTATE.dragging;
        }
        else
        {
            CurrentMouseState = MOUSESTATE.idle;
        }

        if (CurrentMouseState == MOUSESTATE.dragging)
        {
            if (Input.mousePosition.x % 25f == 1)
            {
                print("MODULO");
                if (ScaledMousePosition.x < MouseClickPos.x)
                {
                    CharacterCircle.MoveLeft();
                }
                else if (ScaledMousePosition.x > MouseClickPos.x)
                {
                    CharacterCircle.MoveRight();
                }
            }
        }
    }


    public void LateUpdate()
    {

    }

    public void UpdateAvailableAbilities(SO_AbilityData[] _Abilities)
    {
        CurrentAbilities = _Abilities;


        for (int i = 0; i < AbilityArea.transform.childCount; i++)
        {
            Destroy(AbilityArea.transform.GetChild(i).gameObject);
        }

        if (CurrentAbilities.Length > 0)
        {
            for (int i = 0; i < CurrentAbilities.Length; i++)
            {
                int AbilityIDX = i;
                GameObject Btn = Instantiate(AbilityButtonPrefab, AbilityArea.transform);
                Btn.GetComponent<Button>().onClick.AddListener( delegate { ActivateAttack(CurrentAbilities[AbilityIDX]); } );
                Btn.transform.GetChild(0).GetComponent<Image>().sprite = CurrentAbilities[i].Icon;
            }
        }
    }

    void ActivateMovement()
    {
        CharacterManager SelectedCharacter = CombatSceneManager.Instance.CurrentlySelectedCharacter;
        SelectedCharacter.State = CharacterManager.CharacterState.Moving;
        CombatGridManager.Instance.GenerateMovementCircle(SelectedCharacter.gameObject.transform.position, SelectedCharacter.CharacterData.MoveDistance);
        SetAttackMenuOpenState(false);
    }

    void ActivateAttack(SO_AbilityData _Ability)
    {

        CombatSceneManager.Instance.ClearTargetingInstances();
        switch (_Ability.Cast)
        {
            case SO_AbilityData.CASTTYPE.PINPOINT:
                List<CharacterManager> characters = CombatSceneManager.Instance.GetTargetableCharacters(CombatSceneManager.Instance.CurrentlySelectedCharacter.transform.position, _Ability.TargetRange);
                CombatSceneManager.Instance.CurrentlyTargetedCharacters = characters;
                CombatSceneManager.Instance.CurrentAbility = _Ability;
                print(characters.Count + " CHARACTERS FOUND");
                
                if(characters.Count > 0)
                {
                    CombatSceneManager.Instance.CurrentlySelectedCharacter.State = CharacterManager.CharacterState.PlanningAttack;
                    for (int i = 0; i < characters.Count; i++)
                    {
                        if (characters[i].CurrentTeam == CombatSceneManager.Instance.CurrentlySelectedCharacter.CurrentTeam)
                            return;
                        GameObject PinpointInstance = Instantiate(_Ability.TargetingObject);
                        PinpointInstance.transform.position = characters[i].transform.position;
                        CombatSceneManager.Instance.TargetingInstances.Add(PinpointInstance);
                    }
                }
                break;
            case SO_AbilityData.CASTTYPE.AREA:
                CharacterManager SelectedCharacter = CombatSceneManager.Instance.CurrentlySelectedCharacter;
                SelectedCharacter.State = CharacterManager.CharacterState.Attacking;
                CombatGridManager.Instance.GenerateMovementCircle(SelectedCharacter.gameObject.transform.position, _Ability.TargetRange);
                break;
        }

    }


    public void AttackMenuButtonPressed()
    {
        AttackMenuOpen = !AttackMenuOpen;
        TargetAttackMenuPosition = AttackMenuOpen ? AttackMenuOpenPosition : AttackMenuClosedPosition;
    }

    public void SetAttackMenuOpenState(bool Open)
    {
        AttackMenuOpen = Open;
        TargetAttackMenuPosition = AttackMenuOpen ? AttackMenuOpenPosition : AttackMenuClosedPosition;
    }

}
