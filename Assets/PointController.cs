
using System.Collections.Generic;
using UnityEngine;
using static Unity.Mathematics.math;



[RequireComponent(typeof(AudioSource))]
public class PointController : MonoBehaviour
{
    public int numberOfPoints;
    public float res;
    public GameObject pointTemplate;
    public bool RenderLines = false;
    public bool RunSimulation = false;
    public List<GameObject> linesCreated = new List<GameObject>();
    public List<GameObject> pointsCreated = new List<GameObject>();
    int[,] points;
    public float simulationTime;
    public float timer;
    //public float xInc = 0;
    //public float yInc = 0;

    //public float zInc = 0;

    //private float zoff = 0;
    //public float INC_VALUE;

    private AudioSource _audioSource;
    public float[] _samples;

    // Start is called before the first frame update
    void Start()
    {
        //Random.InitState(42);
        Application.targetFrameRate = 120;
        InitializePoints();
        _samples = new float[numberOfPoints * numberOfPoints];
        _audioSource = GetComponent<AudioSource>();
        timer = simulationTime;
    }

    void InitializePoints()
    {
        points = new int[numberOfPoints, numberOfPoints];
        for (int x = 0; x < numberOfPoints; x++)
        {
            for (int y = 0; y < numberOfPoints; y++)
            {
                GameObject obj = Instantiate(pointTemplate, new Vector3(x * res, y * res, 0), Quaternion.identity);
            }
        }
    }

    void MarchTheSquares()
    {
        GetSpectrumAudioSource();
      
        var curMin = float.MaxValue;
        var curMax = float.MinValue;
        for (var x = 0; x < numberOfPoints; ++x)
        {
            curMin = min(curMin, _samples[x]);
            curMax = max(curMax, _samples[x]);
        }


        int size = numberOfPoints; // Assume numberOfPoints is defined somewhere
        var elements = size * size;
        int[][] points = new int[size][]; // Assume points is defined somewhere
        for (int index = 0; index < size; index++)
        {
            points[index] = new int[size];
        }

        int myRow, myCol;
        if (size % 2 == 0)
        {
            myRow = size / 2 - 1;
            myCol = size / 2;
        }
        else
        {
            myRow = myCol = size / 2;
        }

        int steps = 1; 
        bool increaseSteps = false;
        int direction = 0;
        int count = 0;

       
        float curMatVal = _samples[count++];
        float normalized = ((curMatVal - curMin) / (curMax - curMin));
        points[myRow][myCol] = (30 * normalized >= 0.03f ? 1 : 0);

        while (count < elements)
        {
            for (int step = 0; step < steps; step++)
            {
                switch (direction)
                {
                    case 0:
                        myCol++;
                        break; // Move right
                    case 1:
                        myRow++;
                        break; // Move down
                    case 2:
                        myCol--;
                        break; // Move left
                    case 3:
                        myRow--;
                        break; // Move up
                }

                // Check boundaries
                if (myRow < 0 || myRow >= size || myCol < 0 || myCol >= size) continue;

                // Place element
                curMatVal = _samples[count++];
                normalized = ((curMatVal - curMin) / (curMax - curMin));
                points[myRow][myCol] = (30 * normalized >= 0.03f ? 1 : 0);

                if (count >= elements) break;
            }

            if (increaseSteps) steps++; // Increase steps every other direction change
            increaseSteps = !increaseSteps; // Toggle flag every direction

            direction = (direction + 1) % 4; // Cycle through directions
        }




        for (int x = 0; x < numberOfPoints - 1; x++)
        {
            for (int y = 0; y < numberOfPoints - 1; y++)
            {
                float half = res / 2.0f;
                Vector3 a = new Vector3(x * res + half, y * res, 0);
                Vector3 b = new Vector3(x * res + res, y * res + half, 0);
                Vector3 c = new Vector3(x * res + half, y * res + res, 0);
                Vector3 d = new Vector3(x * res, y * res + half, 0);

                int currentCase = points[x][y] * 1 + points[x + 1][y] * 2 + points[x + 1][y + 1] * 4 +
                                  points[x][y + 1] * 8;
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

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }


    void DrawLine(Vector3 point1, Vector3 point2)
    {
        GameObject lineHolder = new GameObject();
        lineHolder.transform.parent = transform;

        LineRenderer lineRenderer = lineHolder.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Set the number of points in the line
        lineRenderer.startWidth = res / 10;
        lineRenderer.endWidth = res / 10;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new UnityEngine.Color(1, 1, 1);
        lineRenderer.endColor = new UnityEngine.Color(1, 1, 1);


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
        // timer -= Time.deltaTime;
        // if (timer <= 0)
        // {
        //     RunSimulation = true;
        //     timer = simulationTime;
        // }

        // if (RenderLines)
        // {
            DestroyPoints();
            // DestroyLines();
            // MarchTheSquares();
            // RenderLines = false;
        // }

        // if (RunSimulation)
        // {
            DestroyLines();
            MarchTheSquares();
            //RunSimulation = false;
        // }
        
        if (!_audioSource.isPlaying)
        {
            Application.Quit();
        }
    }
}