using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public Camera cam;
    public GameObject particles;
    public GameObject woodCountText;
    public GameObject lostWoodText;
    public Text finalWoodText;
    public GameObject GameOverUI;
    public float particleTime;
    public float tolerance;
    public int woodCount = 0;
    public int lostCount;
    [HideInInspector] public Text lostText;

    private Vector3 _startPos;
    private Vector3 _endPos;
    private bool _isCutting = false;
    private float _minDiagonal = 0.1f;
    private float _maxDiagonal = 0.9f;
    private Text _woodText;
    private Collider2D _playerCollider;

    void Start()
    {
        cam = Camera.main;
        _woodText = woodCountText.GetComponent<Text>();
        lostText = lostWoodText.GetComponent<Text>();
        _playerCollider = gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0)) {
            _isCutting = true;
        }

        if (_isCutting) {
            _playerCollider.enabled = true;
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
        } else {
            _playerCollider.enabled = false;
        }

        if (Input.GetMouseButtonUp(0) && _isCutting)
        {
            _isCutting = false;
        }
        _woodText.text = "Madeira Obtida: " + woodCount.ToString();
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
        } else
        {
            if(desired.x == -1)
            {
                if (drawn.x < -_minDiagonal && drawn.x > -_maxDiagonal && drawn.y < -_minDiagonal && drawn.y > -_maxDiagonal)
                    return true;
                else if (drawn.x > _minDiagonal && drawn.x < _maxDiagonal && drawn.y > _minDiagonal && drawn.y < _maxDiagonal)
                    return true;
            }
            else
            {
                if (drawn.x < -_minDiagonal && drawn.x > -_maxDiagonal && drawn.y > _minDiagonal && drawn.y < _maxDiagonal)
                    return true;
                else if (drawn.x > _minDiagonal && drawn.x < _maxDiagonal && drawn.y < -_minDiagonal && drawn.y > -_maxDiagonal)
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
            WoodMinigame woodObject = collision.gameObject.GetComponent<WoodMinigame>();
            if (CheckCut(woodObject.desiredDir, direction / Vector3.Magnitude(direction)))
            {
                GameObject cutFX = Instantiate(particles, collision.gameObject.transform.position, Quaternion.identity);
                //Destroy(woodObject.line.gameObject);
                Destroy(collision.gameObject);
                woodCount++;
                AudioManager.instance.Play("Chop_Wood");
                StartCoroutine(DestroyParticle(cutFX));
            }
            else
            {
                Debug.Log("Corte errado");
                lostCount--;
                lostText.text = "Erros restantes: " + lostCount.ToString();
                if (lostCount == 0)
                    EndWoodMinigame();
            }
                

        }
    }


    public void EndWoodMinigame()
    {
        GameOverUI.SetActive(true);
        finalWoodText.text = "Você ganhou " + woodCount + "madeiras";
        Time.timeScale = 0f;
    }

    private IEnumerator DestroyParticle(GameObject cutFX)
    {
        yield return new WaitForSeconds(particleTime);
        Destroy(cutFX);
    }


}
