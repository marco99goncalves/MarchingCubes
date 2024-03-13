using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static Unity.Mathematics.math;
using Unity.Mathematics;

public class PointController : MonoBehaviour
{
    public int numberOfPoints;
    public int res;
    public GameObject pointTemplate;
    public bool RenderLines = false;
    public List<GameObject> linesCreated = new List<GameObject>();
    public List<GameObject> pointsCreated = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Random.InitState(42);
        MarchTheSquares();
    }

    void MarchTheSquares()
    {
        int[,] points = new int[numberOfPoints, numberOfPoints];

        // Draw the points
        float xInc = 0;
        int seed = UnityEngine.Random.Range(0, 10000);

        for (int x = 0; x < numberOfPoints; x++)
        {
            float yInc = 0;
            for (int y = 0; y < numberOfPoints; y++)
            {
                GameObject obj = Instantiate(pointTemplate, new Vector3(x, y, 0), Quaternion.identity);
                points[x, y] = noise.snoise(new Vector2(x + seed, y + seed)) > 0.5f ? 1 : 0;
                Debug.Log(points[x, y]);
                
                //points[row, col] = Mathf.PerlinNoise(sRow, sCol) > 0.5 ? 1 : 
                if (points[x, y] == 0)
                {
                    obj.GetComponent<Renderer>().material.color = Color.black;
                }
                pointsCreated.Add(obj);
                yInc += 0.1f;
            }
            xInc += 0.1f;
        }

        for (int x = 0; x < numberOfPoints - 1; x++)
        {
            for (int y = 0; y < numberOfPoints - 1; y++)
            {
                float half = res / 2.0f;
                Vector3 a = new Vector3(x + half, y, 0);
                Vector3 b = new Vector3(x + 1, y + half, 0);
                Vector3 c = new Vector3(x + half, y + 1, 0);
                Vector3 d = new Vector3(x, y + half, 0);

                int currentCase = points[x, y] * 1 + points[x + 1, y] * 2 + points[x + 1, y + 1] * 4 + points[x, y + 1] * 8;
                DrawCase(a, b, c, d, currentCase);
            }
        }
    }

    void DrawCase(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int currentCase)
    {
        switch (currentCase)
        {
            case 1:
                DrawLine(a, d);
                break;
            case 2:
                DrawLine(b, a);
                break;
            case 3:
                DrawLine(b, d);
                break;
            case 4:
                DrawLine(c, b);
                break;
            case 5:
                DrawLine(c, d);
                DrawLine(a, b);
                break;
            case 6:
                DrawLine(c, a);
                break;
            case 7:
                DrawLine(c, d);
                break;
            case 8:
                DrawLine(d, c);
                break;
            case 9:
                DrawLine(a, c);
                break;
            case 10:
                DrawLine(b, c);
                DrawLine(a, d);
                break;
            case 11:
                DrawLine(b, c);
                break;
            case 12:
                DrawLine(b, d);
                break;
            case 13:
                DrawLine(a, b);
                break;
            case 14:
                DrawLine(a, d);
                break;
        }
    }

    void DrawLine(Vector3 point1, Vector3 point2)
    {
        GameObject lineHolder = new GameObject();
        lineHolder.transform.parent = transform; 

        LineRenderer lineRenderer = lineHolder.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Set the number of points in the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Set the start and end points of the line
        lineRenderer.SetPosition(0, point1); // Start point
        lineRenderer.SetPosition(1, point2); // End point
        linesCreated.Add(lineHolder);
    }

    void DestroyLines()
    {
        foreach (GameObject line in linesCreated)
        {
            Destroy(line);
        }
        linesCreated.Clear();
    }

    void DestroyPoints()
    {
        foreach (GameObject point in pointsCreated)
        {
            Destroy(point);
        }
        pointsCreated.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(RenderLines)
        {
            DestroyPoints();
            DestroyLines();
            MarchTheSquares();
            RenderLines = false;
        }
        
    }
}
