using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public Button MoveButton;
    public GameObject AbilityButtonPrefab;
    public GameObject AbilityArea;
    public SO_AbilityData[] CurrentAbilities;
    public static CombatUIManager Instance;

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
    }

    void ActivateAttack(SO_AbilityData _Ability)
    {

        CombatSceneManager.Instance.ClearTargetingInstances();
        switch (_Ability.Cast)
        {
            case SO_AbilityData.CASTTYPE.PINPOINT:
                List<CharacterManager> characters = CombatSceneManager.Instance.GetTargetableCharacters(CombatSceneManager.Instance.CurrentlySelectedCharacter.transform.position, _Ability.TargetRange);
                CombatSceneManager.Instance.CurrentlyTargetedCharacters = characters;
                print(characters.Count + " CHARACTERS FOUND");
                
                if(characters.Count > 0)
                {
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

}
