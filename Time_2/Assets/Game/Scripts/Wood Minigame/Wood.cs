using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public Vector3 desiredDir;
    // Start is called before the first frame update
    void Start()
    {
        desiredDir = setDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 setDirection()
    {
        int num = UnityEngine.Random.Range(0, 4);
        switch(num) {
            case 0:
                return Vector3.up;
            case 1:
                return Vector3.right;
            case 2:
                return new Vector3(1, 1, 0);
            default:
                return new Vector3(-1, 1, 0);
        }
    }
}
