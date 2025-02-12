using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIndicatorManager : MonoBehaviour
{
    public Vector3 SelectorOffset;
    private GameObject IndicatorSprite;
    private Vector3 SelectorPosition = Vector3.zero;
    [SerializeField]
    private float Amplitude;
    [SerializeField]
    private float Frequency;
    private float index;


    private void Start()
    {
        IndicatorSprite = transform.GetChild(0).gameObject;
        SelectorPosition = IndicatorSprite.transform.position;
    }

    private void Update()
    {

        index += Time.deltaTime;
        float sin = Amplitude * Mathf.Sin(index * Frequency);
        SelectorPosition.y = sin;
        IndicatorSprite.transform.position = transform.position + SelectorOffset + SelectorPosition;
    }
}
