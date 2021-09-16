using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public Vector3 desiredDir;
    private float rotationDir;
    public GameObject linePrefab;
    public GameObject line;

    private float lineAngle = 0f;
    // Start is called before the first frame update
    void Start()
    {
        desiredDir = setDirection(ref lineAngle);
        rotationDir = UnityEngine.Random.Range(0, 2);
        if (rotationDir == 0)
        {
            rotationDir = -1;
        }

        line = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
        line.transform.Rotate(0f, 0f, lineAngle);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 100*rotationDir * Time.deltaTime);
        if (transform.position.y < -10)
        {
            //Destroy(line.gameObject);
            Destroy(gameObject);
        }

        //line.transform.position = transform.position;
        //line.transform.rotation = Quaternion.Euler(0f, 0f, lineAngle);
    }

    Vector3 setDirection(ref float lineAngle)
    {
        int num = UnityEngine.Random.Range(0, 4);
        switch(num) {
            case 0:
                lineAngle = 90f;
                return Vector3.up;
            case 1:
                lineAngle = 0f;
                return Vector3.right;
            case 2:
                lineAngle = 135f;
                return new Vector3(1, 1, 0);
            default:
                lineAngle = 45f;
                return new Vector3(-1, 1, 0);
        }
    }
}
