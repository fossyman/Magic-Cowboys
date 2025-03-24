using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSpinTest : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] Images;
    public GameObject[] Layers;
    [SerializeField]private Vector3[] Positions;
    public int[] LayerIndexes;
    public Vector2 Radius;
    public float Speed;

    public float RotValue;

    public float RotationDuration;

    public int index;
    private int PrevIndex;

    private float Offset;


    void Start()
    {
        
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
        }

        Layers[index].transform.SetAsLastSibling();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotValue += (Mathf.PI * 2 / Layers.Length);
            PrevIndex = index;
            index++;
            if (index > Layers.Length - 1)
            {
                index = 0;
            }
            if (RotValue >= 360 || RotValue <= -360) { RotValue = 0; }
            Images[PrevIndex].GetComponent<Image>().color = Color.white;
            Images[index].GetComponent<Image>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotValue -= (Mathf.PI * 2 / Layers.Length);
            PrevIndex = index;
            index--;
            if (index < 0)
            {
                index = Layers.Length-1;
            }
            if (RotValue >= 360 || RotValue <= -360) { RotValue = 0; }
            Images[PrevIndex].GetComponent<Image>().color = Color.white;
            Images[index].GetComponent<Image>().color = Color.red;
        }
    }
}
