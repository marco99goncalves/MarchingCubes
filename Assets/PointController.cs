using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public int numberOfPoints;
    public GameObject pointTemplate;

    // Start is called before the first frame update
    void Start()
    {
        // Draw the points
        for (int row = 0; row < numberOfPoints; row++)
        {
            for (int col = 0; col < numberOfPoints; col++)
            {
                Instantiate(pointTemplate, new Vector3(row, col, 0), Quaternion.identity);
            }
        }

        for(int row = 0; row < numberOfPoints; row++)
        {
            for(int col = 0; col < numberOfPoints; col++)
            {

            }
        }
    }

    void DrawLine(Vector3 point1, Vector3 point2)
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Set the number of points in the line

        // Set the start and end points of the line
        lineRenderer.SetPosition(0, point1); // Start point
        lineRenderer.SetPosition(1, point2); // End point
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
