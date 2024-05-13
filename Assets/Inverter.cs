using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    public MarchingCubes marcher;
    private float res;
    
    // Start is called before the first frame update
    void Start()
    {
        marcher = FindObjectOfType<MarchingCubes>();
        res = marcher.res;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Point")
        {
            Vector3 pointPos = other.gameObject.transform.position;
            marcher.points[(int)(pointPos.x / res), (int)(pointPos.y / res), (int)(pointPos.z / res)] = 1;
                //marcher.points[(int)(pointPos.x / res), (int)(pointPos.y / res), (int)(pointPos.z / res)] == 0 ? 1 : 0;
        }
    }
}
