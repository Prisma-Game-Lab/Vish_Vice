using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    public float woodTimer;
    public GameObject woodPrefab;
    private int _woodCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WoodSpawn());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator WoodSpawn()
    {
        while (true)
        {
            float woodX = UnityEngine.Random.Range(-10.5f, 10.5f);
            GameObject tempWood = Instantiate(woodPrefab, new Vector3(woodX, -6, 0), Quaternion.identity);
            _woodCounter++;
            float forceX = 0f;
            if (woodX > 0) {
                forceX = UnityEngine.Random.Range(-1f, 0f);
            } else {
                forceX = UnityEngine.Random.Range(0f, 1f);
            }
            tempWood.GetComponent<Rigidbody2D>().AddForce((Vector3.up + new Vector3(forceX,0,0))* 600);
            yield return new WaitForSeconds(woodTimer);
        }
    }
}
