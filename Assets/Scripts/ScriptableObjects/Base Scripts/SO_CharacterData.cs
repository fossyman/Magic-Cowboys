using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Components/Character Data", order = 1)]
public class SO_CharacterData: ScriptableObject
{
    [Range(0,5)]
    public int MoveDistance;

    public SO_MDSprite IdleSprite;

}
