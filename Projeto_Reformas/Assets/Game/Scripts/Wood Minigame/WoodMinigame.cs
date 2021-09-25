using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMinigame : MonoBehaviour
{
    public Vector3 desiredDir;
    public GameObject linePrefab;
    public GameObject line;
    public Slice slice;

    private float lineAngle = 0f;
    private float rotationDir;
    void Start()
    {
        desiredDir = setDirection(ref lineAngle);
        if (transform.position.x < 0)
            rotationDir = -1;
        else
            rotationDir = 1;

        line = Instantiate(linePrefab, transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 80*rotationDir * Time.deltaTime);
        if (transform.position.y < -10)
            Destroy(gameObject);

        line.transform.rotation = Quaternion.Euler(0f, 0f, lineAngle);
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
