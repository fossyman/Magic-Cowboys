using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Data", menuName = "Components/Ability Data", order = 1)]
public class SO_AbilityData : ScriptableObject
{
    public Sprite Icon;

    public enum EFFECTTYPE {DAMAGE,HEALING,SHIELD }
    public EFFECTTYPE Effect;

    public enum CASTTYPE { AREA, PINPOINT, SHIELD }
    public CASTTYPE Cast;

    public enum MODIFIERTYPES { NONE, FLAME, POISON, ICE, ELECTRIC, WIND, WATER }
    public MODIFIERTYPES[] Modifiers;

    public int TargetRange;

    public int Amount;

    public int CooldownTime;
    
}
