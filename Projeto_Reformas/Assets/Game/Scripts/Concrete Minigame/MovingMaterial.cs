using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingMaterial : MonoBehaviour
{
    [HideInInspector]public int speed;
    [HideInInspector]public bool isMovingSide = true;
    [HideInInspector] public bool isMovingDown = false;
    [HideInInspector]public int type;
    [HideInInspector] public int treadmill;
    private BoxCollider2D bc;

    private void OnMouseDrag()
    {
        isMovingSide = false;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3
            (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseUp()
    {
        bc.enabled = false;
        isMovingDown = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingSide) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        } else if (isMovingDown) {
            transform.Translate(0, -20 * Time.deltaTime, 0);
        }
    }
}
