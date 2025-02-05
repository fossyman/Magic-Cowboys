using System;
using System.Collections.Generic;
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

    void MovePlayer()
    {
    
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
        for( int i = 0; i< Gridsquares.Count; i++)
        {
            Destroy(Gridsquares[i]);
        }
        List<Vector3> Positions = new List<Vector3>();

        int rotationstep = 360 / 8;
        for (float x = 0; x <= 8; x ++)
        {

            for (float y = 0; y < MaxMoveDistance / (x * Mathf.Deg2Rad); y++)
            {
                var ang = rotationstep * x;
                Vector3 Startingpos = Player.transform.position + new Vector3(Mathf.RoundToInt(Mathf.Cos(ang * Mathf.Deg2Rad)),0.5f, Mathf.RoundToInt(Mathf.Sin(ang * Mathf.Deg2Rad))) * y;
                RaycastHit hit;
                Debug.DrawRay(Startingpos, Vector3.down * 100f, Color.red, 100);
                if (Physics.Raycast(Startingpos, Vector3.down,out hit, 5f, layerMask))
                {
                    Debug.DrawRay(Startingpos, Vector3.down * 100f, Color.green, 100);
                    Positions.Add(hit.point);
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
