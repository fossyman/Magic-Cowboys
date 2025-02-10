using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Multi Directional Sprite", menuName = "Components/Multi Directional Sprite", order = 1)]
public class SO_MDSprite:ScriptableObject
{
    [Tooltip("Keeps a collection of sprites. MAKE SURE TO START WITH THE FACING FORWARD SPRITE AND ADD ADDITIONAL IMAGES CLOCKWISE")]
    public Sprite[] textures;
}
