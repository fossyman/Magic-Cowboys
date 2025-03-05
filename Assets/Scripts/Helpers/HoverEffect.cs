using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    float CurrentTimeValue = 0;
    Vector3 CurrentHoverPosition;
    public float Amt;
    public float Speed = 1.0f;
    public float Amp = 1.0f;
    public Vector3 Offset;
    // Update is called once per frame
    private void Start()
    {
        CurrentHoverPosition = transform.position;
    }
    void Update()
    {
        CurrentTimeValue += Time.deltaTime;
        CurrentHoverPosition.y = Offset.y + Amp * Mathf.Sin(Amt * Mathf.PI * Speed * CurrentTimeValue);
        transform.position = CurrentHoverPosition;
    }
}
