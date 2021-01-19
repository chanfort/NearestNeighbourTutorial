using UnityEngine;
using System.Collections.Generic;

public class Neighbours : MonoBehaviour
{
    public GameObject prefab;
    public int n = 10;

    public List<GameObject> gos = new List<GameObject>();
    List<Vector3> positions = new List<Vector3>();
    public List<GameObject> neighb = new List<GameObject>();

    KDTree kd;

    float contr_sum1 = 0f;
    float contr_sum2 = 0f;

    bool useKd = false;

    void Start()
    {
        Random.InitState(1);

        for (int i = 0; i < n; i++)
        {
            Vector2 pos2d = 5f * Random.insideUnitCircle;
            Vector3 pos = new Vector3(pos2d.x, 0f, pos2d.y);
            positions.Add(pos);
        }

        kd = KDTree.MakeFromPoints(positions.ToArray());

        FindNearestNeighbours();
        FindNearestNeighboursKD();

        Debug.Log(contr_sum1 + " " + contr_sum2);
    }

    void FindNearestNeighbours()
    {

        for (int i = 0; i < n; i++)
        {
            float minDist = 999999f;
            int min_id = -1;

            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    float dist = (positions[i] - positions[j]).magnitude;
                    if (dist < minDist)
                    {
                        minDist = dist;
                        min_id = j;
                    }
                }
            }

            contr_sum1 = contr_sum1 + minDist;
        }
    }

    void FindNearestNeighboursKD()
    {
        for (int i = 0; i < n; i++)
        {
            int min_id = kd.FindNearestK(positions[i], 1);
            float dist = (positions[i] - positions[min_id]).magnitude;
            contr_sum2 = contr_sum2 + dist;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            useKd = !useKd;
        }

        if (useKd == false)
        {
            FindNearestNeighbours();
        }
        else
        {
            FindNearestNeighboursKD();
        }
    }
}
