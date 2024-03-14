using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private int poolStartSize = 100;

    private Queue<GameObject> linePool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject obj = Instantiate(linePrefab);
            obj.SetActive(false);
            obj.transform.parent = transform; // Optionally parent to a specific transform for organization
            linePool.Enqueue(obj);
        }
    }

    public GameObject GetLine()
    {
        if (linePool.Count > 0)
        {
            GameObject obj = linePool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return Instantiate(linePrefab); // Fallback if pool is exhausted
        }
    }

    public void ReturnLine(GameObject line)
    {
        line.SetActive(false);
        linePool.Enqueue(line);
    }
}

