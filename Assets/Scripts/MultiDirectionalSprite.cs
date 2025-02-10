using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDirectionalSprite : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer SpriteRenderer;

    private Material CharacterMaterialInstance;

    [SerializeField]
    private Shader CharacterShaderInstance;

    public SO_MDSprite CurrentSprite;

    private float FlipValue = 1.0f;

    void Start()
    {
        CharacterMaterialInstance = new Material(CharacterShaderInstance);
        SpriteRenderer.material = CharacterMaterialInstance;
        RecalculateSpriteDirection();
    }

    // Update is called once per frame
    void Update()
    {
        RecalculateSpriteDirection();
    }


    private void RecalculateSpriteDirection()
    {
        Vector3 CamForward = Camera.main.transform.forward;
        var Forward = SpriteRenderer.transform.forward;
        var Left = -SpriteRenderer.transform.right;

        float LDot = Vector3.Dot(Left, CamForward);
        float FDot = Vector3.Dot(Forward, CamForward);

        float RotationIDX = GetRotationIndex(LDot, FDot);
        CharacterMaterialInstance.SetFloat("_FlipValue", FlipValue);

        if (RotationIDX != -1)
        {
            switch (RotationIDX)
            {
                case 0:
                    CharacterMaterialInstance.SetTexture("_CurrentSprite", CurrentSprite.textures[0].texture);
                    break;
                case 1:
                    CharacterMaterialInstance.SetTexture("_CurrentSprite", CurrentSprite.textures[1].texture);
                    break;
                case 2:
                    CharacterMaterialInstance.SetTexture("_CurrentSprite", CurrentSprite.textures[2].texture);
                    break;
                case 3:
                    CharacterMaterialInstance.SetTexture("_CurrentSprite", CurrentSprite.textures[3].texture);
                    break;
                default:
                    CharacterMaterialInstance.SetTexture("_CurrentSprite", CurrentSprite.textures[3].texture);

                    break;

            }
        }
    }


    int GetRotationIndex(float LDOT, float FDOT)
    {
        print("LDOT: " + Mathf.Abs(LDOT));
        print("FDOT: " + Mathf.Abs(FDOT));

        if (FDOT < -0.58) // LEFT
            return 1;
        else if (FDOT > 0.59f)
            return 3;
        else
        {
            FlipValue = LDOT > 0 ? -1 : 1;
            if (Mathf.Abs(FDOT) < 0.7f)
                return 0;
            else
                return 0;
        }

    }
}
