using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Transform sun;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        RotateSun();
    }


    void RotateSun()
    {
        float timer = Time.deltaTime;
        sun.transform.Rotate(timer * speed, 0, 0);
        timer = 0f;
    }
}
