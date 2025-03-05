using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CombatCameraManager : MonoBehaviour
{
    public static CombatCameraManager Instance;

    public GameObject CameraTarget;

    private Vector3 CameraVelocity;

    float Smoothtime = 0.5f;

    public void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    public void Update()
    {
        if (CameraTarget != null)
        {
            if (transform.position != CameraTarget.transform.position)
            {
                transform.position = Vector3.SmoothDamp(transform.position, CameraTarget.transform.position, ref CameraVelocity, Smoothtime);
            }
        }


    }
    public void SetCameraTarget(GameObject _target)
    {
        CameraTarget = _target;
        MoveCameraToTarget(true);
    }

    public void MoveCameraToTarget(bool IsSmooth, float _Smoothness = 0.2f)
    {
        if (IsSmooth)
        {
            Smoothtime = _Smoothness;
        }
        else
        {
            transform.position = CameraTarget.transform.position;
        }
    }
}
