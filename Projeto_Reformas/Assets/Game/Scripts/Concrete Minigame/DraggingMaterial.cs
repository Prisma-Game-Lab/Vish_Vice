using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingMaterial : MonoBehaviour
{
    public Camera cam;
    private bool _isDragging;
    private Collider2D _playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        _isDragging = false;
        _playerCollider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _isDragging = true;

        if (_isDragging)
        {
            _playerCollider.enabled = true;
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
        }
        else
            _playerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            collision.gameObject.GetComponent<MovingMaterial>().isMoving = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            collision.transform.position = transform.position;
        }
    }
}
