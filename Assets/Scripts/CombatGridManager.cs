using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CombatGridManager : MonoBehaviour
{

    public static CombatGridManager Instance { get; private set; }

    public GameObject Player;
    public int MaxMoveDistance;
    public int MaxHeightDistance;

    public Vector3[] CurrentPositionPossibilities;
    public Vector3[] CurrentRotationPossibilities;

    public GameObject Gridvisual;

    public LayerMask layerMask;

    public Vector3 RaycastOffset = new Vector3(0.4f, 0.5f, 0.4f);

    public List<GameObject> Gridsquares = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CombatSceneManager.Instance.CurrentlySelectedCharacter)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit,Mathf.Infinity,layerMask);

            if (hasHit)
            {
                Vector3 Gridsquare;
                float Normal = Vector3.Angle(Vector3.up, hit.normal);
                print(Normal * Mathf.Deg2Rad);
                if (Normal > 0)
                {
                    Gridsquare = new Vector3(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y) +0.5f, Mathf.FloorToInt(hit.point.z));
                }
                else
                {
                    Gridsquare = new Vector3(Mathf.FloorToInt(hit.point.x), hit.point.y, Mathf.FloorToInt(hit.point.z));
                }

                switch (CombatSceneManager.Instance.CurrentlySelectedCharacter.State)
                {
                    case CharacterManager.CharacterState.Idle:
                        ClearGridVisuals();
                        break;
                    case CharacterManager.CharacterState.Moving:
                        CombatSceneManager.Instance.CurrentlySelectedCharacter.AttemptMoveToNewPoint(Gridsquare);
                        ClearGridVisuals();
                        break;
                    case CharacterManager.CharacterState.Attacking:
                        ClearGridVisuals();
                        break;
                    case CharacterManager.CharacterState.Dead:
                        ClearGridVisuals();
                        break;
                }
            }
        }
    }

    public void ClearGridVisuals()
    {
        for (int i = 0; i < Gridsquares.Count; i++)
        {
            Destroy(Gridsquares[i]);
        }
    }


    public void GenerateMovementCircle(Vector3 _Position,int _Radius)
    {
        Vector3 PlayerGridPosition = new Vector3(Mathf.RoundToInt(_Position.x), Mathf.RoundToInt(_Position.y), Mathf.RoundToInt(_Position.z));
        ClearGridVisuals();
        Gridsquares.Clear();
        List<Vector3> Positions = new List<Vector3>();
        List<Vector3> Rotations = new List<Vector3>();
        for (int x = (int)PlayerGridPosition.x - _Radius; x < (int)PlayerGridPosition.x + _Radius + 1; x++)
        {
            for (int y = (int)PlayerGridPosition.y - _Radius; y < (int)PlayerGridPosition.y + _Radius; y++)
            {
                for (int z = (int)PlayerGridPosition.z - _Radius; z < (int)PlayerGridPosition.z + _Radius + 1; z++)
                {
                    RaycastHit hit;
                    Vector3 RaycastOrigin = new Vector3(x + RaycastOffset.x, y + RaycastOffset.y, z + RaycastOffset.z);
                    bool hasHit = Physics.Raycast(RaycastOrigin, Vector3.down, out hit, Mathf.Infinity, layerMask);
                    Debug.DrawRay(RaycastOrigin, Vector3.down, Color.white, 5);
                    if (hasHit)
                    {
                        Debug.DrawRay(RaycastOrigin, Vector3.down, Color.green, 5);
                        Vector3 FoundValue = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        Vector3 RoundedFoundValue = new Vector3(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));

                        print("PATH " + RoundedFoundValue + " PLAYER " + PlayerGridPosition);
                        // PATH FOUND
                        if (Vector3.Distance(FoundValue, PlayerGridPosition) < _Radius + 2 && (RoundedFoundValue.x != PlayerGridPosition.x || RoundedFoundValue.z != PlayerGridPosition.z))
                        {
                            print(hit.normal);
                            // PATH WITHIN REACH
                            Debug.DrawRay(FoundValue + new Vector3(0,1,0), Vector3.down, Color.magenta, 5);
                            if (!Positions.Contains(RoundedFoundValue))
                            {
                                if (hit.normal.y == 1f)
                                {
                                    Positions.Add(new Vector3(RoundedFoundValue.x, RoundedFoundValue.y, RoundedFoundValue.z));
                                    Rotations.Add(Vector3.zero);
                                }
                                else
                                {
                                    Positions.Add(new Vector3(RoundedFoundValue.x, RoundedFoundValue.y + 0.2f, RoundedFoundValue.z + 0.1f));
                                    Rotations.Add(new Vector3(-45,0,0));
                                }
                                
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                        // PATH NOT FOUND
                    }
                }
            }
        }

        CurrentPositionPossibilities = Positions.ToArray();
        CurrentRotationPossibilities = Rotations.ToArray();
        for (int x = 0; x < CurrentPositionPossibilities.Length; x++)
        {
            var g = Instantiate(Gridvisual);
            g.transform.position = CurrentPositionPossibilities[x];
            g.transform.rotation = Quaternion.Euler(CurrentRotationPossibilities[x]);
            g.name = CurrentPositionPossibilities[x].ToString();

            Gridsquares.Add(g);
        }
    }

    public Vector3[] CalculateGridDistance(Vector3 _value1, Vector3 _value2, float _distanceValue = 1)
    {
        Vector3 val1 = CalculateGridSquare(_value1);
        Vector3 val2 = CalculateGridSquare(_value2);
        float Distance = Vector2.Distance(new Vector2(val1.x, val1.z), new Vector2(val2.x, val2.z));
        print("CHECKING DISTANCE OF :  " + Distance);
        Debug.DrawRay(val1, Vector3.down, Color.blue, 100);
        Debug.DrawRay(val2, Vector3.down, Color.red, 100);
        if (Mathf.RoundToInt(Distance) <= _distanceValue + 0.5f)
        {
            Vector3[] CombinedValues = new Vector3[2];
            CombinedValues[0] = val1;
            CombinedValues[1] = _value2;
            print("MOVEABLE : " + CombinedValues.Length);
            return CombinedValues;
        }
        print("Position is too far");
        return new Vector3[0];
    }

    public Vector3 CalculateGridSquare(Vector3 _value)
    {
        return new Vector3(Mathf.RoundToInt(_value.x), Mathf.RoundToInt(_value.y), Mathf.RoundToInt(_value.z));
    }

}
