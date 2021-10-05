using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMaterial : MonoBehaviour
{
    [HideInInspector]public int speed;
    private bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 11 || transform.position.x < -11)
        {
            Destroy(gameObject);
        }

        if (isMoving) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }
}
