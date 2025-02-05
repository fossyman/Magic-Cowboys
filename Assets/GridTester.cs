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
    public Vector3[] CurrentPossibilities;
    public GameObject Gridvisual;

    public LayerMask layerMask;

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
        //List<Vector3> Positions = new List<Vector3>();

        //float rotationStep = 360 / 8;
        //for (int x = 1; x < MaxMoveDistance; x++)
        //{
        //    for (int y = 0; y < 8; y++)
        //    {
        //        float ang = rotationStep * y;
        //        Vector3 pos = new Vector3(Mathf.Cos(ang * Mathf.Deg2Rad), 0, Mathf.Sin(ang * Mathf.Deg2Rad));
        //        Positions.Add(pos);
        //    }
        //}
        //CurrentPossibilities = Positions.ToArray();
        //for (int x = 0; x < CurrentPossibilities.Length; x++)
        //{
        //    var g = Instantiate(Gridvisual);
        //    g.transform.position = Player.transform.position - CurrentPossibilities[x];
        //}
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

        for (int x = (int)PlayerGridPosition.x - MaxMoveDistance; x < (int)PlayerGridPosition.x + MaxMoveDistance; x++)
        {
            for (int y = (int)PlayerGridPosition.y - MaxMoveDistance; y < (int)PlayerGridPosition.y + MaxMoveDistance; y++)
            {
                for (int z = (int)PlayerGridPosition.z - MaxMoveDistance; z < (int)PlayerGridPosition.z + MaxMoveDistance; z++)
                {
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(new Vector3(x, y+ 0.5f, z), Vector3.down, out hit, Mathf.Infinity, layerMask);
                    if (hasHit)
                    {
                        // PATH FOUND
                        if (Vector3.Distance(hit.point, PlayerGridPosition) <= MaxMoveDistance)
                        {
                            // PATH WITHIN REACH
                            Vector3 FoundValue = new Vector3(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));
                            Positions.Add(FoundValue);
                        }
                        else
                        {
                            // PATH OUTSIDE OF REACH
                        }
                    }
                    else
                    {
                        // PATH NOT FOUND
                    }
                }
            }
        }

        CurrentPossibilities = Positions.ToArray();

        for (int x = 0; x < CurrentPossibilities.Length; x++)
        {
            var g = Instantiate(Gridvisual);
            g.transform.position = CurrentPossibilities[x];

            Gridsquares.Add(g);
        }
    }

}
