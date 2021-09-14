using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public Camera cam;
    public GameObject particles;
    public float particleTime;
    public float tolerance;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isCutting = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0)) {
            _isCutting = true;
        }

        if (_isCutting) {
            gameObject.GetComponent<Collider2D>().enabled = true;
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
        } else {
            gameObject.GetComponent<Collider2D>().enabled = false;
        }

        if (Input.GetMouseButtonUp(0) && _isCutting)
        {
            _isCutting = false;
        }
    }

    private bool CheckCut(Vector3 desired, Vector3 drawn)
    {
        if (Mathf.Abs(desired.x - drawn.x) < tolerance)
        {
            return true;
        } else if (Mathf.Abs(desired.y - drawn.y) < tolerance)
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wood"))
        {
            _startPos = cam.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wood"))
        {
            _endPos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = _endPos - _startPos;
            Wood woodObject = collision.gameObject.GetComponent<Wood>();
            if (CheckCut(woodObject.desiredDir, direction))
            {
                GameObject cutFX = Instantiate(particles, collision.gameObject.transform.position, Quaternion.identity);
                Destroy(collision.gameObject);
                StartCoroutine(DestroyParticle(cutFX));
            }
        }
    }

    private IEnumerator DestroyParticle(GameObject cutFX)
    {
        yield return new WaitForSeconds(particleTime);
        Destroy(cutFX);
    }
}
