using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public Camera cam;
    public GameObject particles;
    public GameObject woodCountText;
    public float particleTime;
    public float tolerance;
    public int woodCount = 0;

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

        woodCountText.GetComponent<Text>().text = "Madeira Obtida: " + woodCount.ToString();
    }

    private bool CheckCut(Vector3 desired, Vector3 drawn)
    {
        if (desired.y == 1 && desired.x == 0) {
            if (Mathf.Abs(desired.x - drawn.x) < tolerance)
            {
                return true;
            }
        } else if (desired.y == 0 && desired.x == 1)
        {
            if (Mathf.Abs(desired.y - drawn.y) < tolerance)
            {
                return true;
            }
        } else if (desired.y == 1 && desired.x != 0)
        {
            float angle = Vector3.SignedAngle(desired, drawn, Vector3.up); //Returns the angle between -180 and 180.
            if (angle < 0) {
                angle = 360 - angle * -1;
            }

            if (angle < 60 || angle > 300) {
                return true;
            }
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
            if (CheckCut(woodObject.desiredDir, direction/Vector3.Magnitude(direction)))
            {
                GameObject cutFX = Instantiate(particles, collision.gameObject.transform.position, Quaternion.identity);
                //Destroy(woodObject.line.gameObject);
                Destroy(collision.gameObject);
                woodCount++;
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
