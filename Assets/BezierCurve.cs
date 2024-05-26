using System;
using UnityEngine;

public class BezierCurveRenderer : MonoBehaviour
{
    public Transform[] controlPoints; // Assign in inspector
    public int curveResolution = 100; // Number of points along the curve
    public GameObject obj; // Object to move along the curve
    public float speed = 1f; // Speed at which obj moves along the curve
    private float t = 0f; // Time parameter for the Bezier curve

    public float posTime = 5f;
    public float negTime = 2f;
    public float timer = 0f;
    public bool isPositive = true;

    public float ignoreTimer = 0f;
    
    void Start()
    {
    }

    private void Update()
    {
        timer += Time.deltaTime;
        ignoreTimer += Time.deltaTime;

        if (ignoreTimer >= 90f)
            Application.Quit();

        if (timer > (isPositive ? posTime : negTime))
        {
            isPositive = !isPositive;
            timer = 0f;
            speed *= -1;
        }
        
        DrawBezierCurve();
        UpdateObjectPosition();
    }

    void DrawBezierCurve()
    {
        Vector3[] points = new Vector3[curveResolution+1];

        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (curveResolution - 1f);
            points[i] = CalculateBezierPoint(t);
        }

        points[curveResolution] = CalculateBezierPoint(curveResolution / (curveResolution - 1f));
    }

    Vector3 CalculateBezierPoint(float t)
    {
        Vector3[] temp = new Vector3[controlPoints.Length];
        for (int i = 0; i < controlPoints.Length; i++)
            temp[i] = controlPoints[i].position;

        int n = controlPoints.Length - 1;
        for (int k = 1; k <= n; k++)
        {
            for (int i = 0; i < n - k + 1; i++)
                temp[i] = (1 - t) * temp[i] + t * temp[i + 1];
        }
        return temp[0];
    }

    void UpdateObjectPosition()
    {
        t += Time.deltaTime * speed;
        if (t > 1f)
            t %= 1f; // Reset t to 0 to loop the movement, or use t %= 1f to smoothly wrap around

        obj.transform.position = CalculateBezierPoint(t);
    }
}