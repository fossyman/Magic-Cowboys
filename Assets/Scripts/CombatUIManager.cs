using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public Button MoveButton;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateMovement()
    {
        CharacterManager SelectedCharacter = CombatSceneManager.Instance.CurrentlySelectedCharacter;
        SelectedCharacter.State = CharacterManager.CharacterState.Moving;
        CombatGridManager.Instance.GenerateMovementCircle(SelectedCharacter.gameObject.transform.position, SelectedCharacter.CharacterData.MoveDistance);
    }

    void ActivateAttack()
    {
        CharacterManager SelectedCharacter = CombatSceneManager.Instance.CurrentlySelectedCharacter;
        SelectedCharacter.State = CharacterManager.CharacterState.Attacking;
        CombatGridManager.Instance.GenerateMovementCircle(SelectedCharacter.gameObject.transform.position, SelectedCharacter.CharacterData.MoveDistance);
    }
}
