using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GridTester : MonoBehaviour
{

    public GameObject Player;
    public int MaxMoveDistance;
    public int MaxHeightDistance;

    public Vector3[] CurrentPositionPossibilities;
    public Vector3[] CurrentRotationPossibilities;

    public GameObject Gridvisual;

    public LayerMask layerMask;

    public Vector3 RaycastOffset = new Vector3(0.4f, 0.5f, 0.4f);

    public List<GameObject> Gridsquares = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Collider[] FoundColliders;
        for (int x = 0; x < transform.childCount; x++)
        {
            for (int y = 0; y < transform.childCount; y++) { }
            FoundColliders = GetComponents<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                //print("REAL: " + hit.point + " GRID VER.: " + Gridsquare);
                Player.transform.position = Gridsquare;
                DetermineMovementDirections(Player.transform.position);
            }
        }
    }

    void DetermineMovementDirections(Vector3 Point)
    {
        GenerateCircle();
    }


    void GenerateCircle()
    {
        Vector3 PlayerGridPosition = new Vector3(Mathf.RoundToInt(Player.transform.position.x), Mathf.RoundToInt(Player.transform.position.y), Mathf.RoundToInt(Player.transform.position.z));
        for( int i = 0; i< Gridsquares.Count; i++)
        {
            Destroy(Gridsquares[i]);
        }
        Gridsquares.Clear();
        List<Vector3> Positions = new List<Vector3>();
        List<Vector3> Rotations = new List<Vector3>();
        for (int x = (int)PlayerGridPosition.x - MaxMoveDistance ; x < (int)PlayerGridPosition.x + MaxMoveDistance + 1; x++)
        {
            for (int y = (int)PlayerGridPosition.y - MaxMoveDistance; y < (int)PlayerGridPosition.y + MaxMoveDistance ; y++)
            {
                for (int z = (int)PlayerGridPosition.z - MaxMoveDistance; z < (int)PlayerGridPosition.z + MaxMoveDistance + 1; z++)
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
                        if (Vector3.Distance(FoundValue, PlayerGridPosition) < MaxMoveDistance + 2 && (RoundedFoundValue.x != PlayerGridPosition.x || RoundedFoundValue.z != PlayerGridPosition.z))
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

}
