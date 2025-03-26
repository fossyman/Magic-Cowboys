using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class CircleSpinTest : MonoBehaviour
{
    // Start is called before the first frame update

    private Image[] Images;
    public GameObject[] Layers;
    [SerializeField]private Vector3[] Positions;
    public int[] LayerIndexes;
    public Vector2 Radius;
    public float Speed;

    public float RotValue;

    public float RotationDuration;

    public int index;
    private int PrevIndex;
    private int NextIndex;
    private int RealIndex;

    private float Offset;


    void Start()
    {
        Images = new Image[Layers.Length];
        for (int i = 0; i < Layers.Length; i++)
        {
            Images[i] = Layers[i].transform.GetChild(0).GetComponent<Image>();
        }

        PrevIndex = index - 1;
        PrevIndex = wrap(PrevIndex, 0, Layers.Length - 1);
        NextIndex = index + 1;
        NextIndex = wrap(NextIndex, 0, Layers.Length - 1);
    }

    // Update is called once per frame
    void Update()
    {
        Offset += Mathf.PI * 2 * Time.deltaTime / RotationDuration;

        if (Offset < Mathf.PI) { Offset = Mathf.PI; } else if (Offset > Mathf.PI) { Offset = -Mathf.PI; }

        var spacing = Mathf.PI * 2 / Layers.Length;

        for (int i = 0; i < Layers.Length; i++)
        {
            //Layers[i].transform.position = transform.position + new Vector3(Mathf.Sin(Time.timeSinceLevelLoad + (360 * i) * Speed) * Radius, Mathf.Cos(Time.timeSinceLevelLoad + (360 * i) * Speed) * Radius, 0);
            Positions[i] = transform.position + new Vector3(Mathf.Sin(spacing * i + Offset + RotValue) * Radius.x, Mathf.Cos(spacing * i + Offset + RotValue) * Radius.y, 0);

            Layers[i].transform.position = Vector3.Lerp(Layers[i].transform.position, Positions[i], 15 * Time.deltaTime);
            Images[i].transform.localScale = i == RealIndex ? Vector3.one : Vector3.one * 0.9f;

            if (i == RealIndex)
            {
                Images[i].color = Color.Lerp(Images[i].color, Color.white, 15 * Time.deltaTime);
            }
            else
            {
                Images[i].color = Color.Lerp(Images[i].color, Color.grey, 15 * Time.deltaTime);
            }
        }

        print(RealIndex);



        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }


    public void MoveLeft()
    {
        RotValue += (Mathf.PI * 2 / Layers.Length);
        PrevIndex = index;
        index++;
        NextIndex = index + 1;
        RealIndex--;
        index = index > Layers.Length - 1 ? 0 : index;
        NextIndex = wrap(NextIndex, 0, Layers.Length - 1);
        PrevIndex = wrap(PrevIndex, 0, Layers.Length - 1);
        if (RealIndex < 0)
        {
            RealIndex = Layers.Length - 1;
        }
        else if (RealIndex > Layers.Length - 1)
        {
            RealIndex = 0;
        }
        if (RotValue >= 360 || RotValue <= -360) { RotValue = 0; }
        Layers[RealIndex].transform.SetAsLastSibling();
    }

    public void MoveRight()
    {
        RotValue -= (Mathf.PI * 2 / Layers.Length);
        PrevIndex = index;
        index--;
        NextIndex = index - 1;
        RealIndex++;

        index = index < 0 ? Layers.Length - 1 : index;

        NextIndex = wrap(NextIndex, 0, Layers.Length - 1);
        PrevIndex = wrap(PrevIndex, 0, Layers.Length - 1);

        if (RealIndex < 0)
        {
            RealIndex = Layers.Length - 1;
        }
        else if (RealIndex > Layers.Length - 1)
        {
            RealIndex = 0;
        }
        if (RotValue >= 360 || RotValue <= -360) { RotValue = 0; }
        Layers[RealIndex].transform.SetAsLastSibling();
    }



    int wrap(int value, int min, int max)
    {
        if (value > max) { return min; }
        if (value < min) { return max; }
        return value;
    }
}
