using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [Header("Intervalo minimo entre o surgimento das madeiras(segundos)")]
    public float minWoodSpawnTime;
    [Header("Intervalo maximo entre o surgimento das madeiras(segundos)")]
    public float maxWoodSpawnTime;
    [Header("A cada quantos segundos decorridos de minigame o tempo entre o surgimento de madeiras deve diminuir(segundos)")]
    public float decreaseTimeInterval;
    [Header("Quantos segundos sao retirados do intervalo(segundos)")]
    public float decreaseRate;
    [Header("Quantidade maxima de madeiras por vez")]
    public int maxQtdWood;
    [Header("A cada tempo decorrido, o tempo entre a quantidade de madeiras que surgem deve aumentar(segundos)")]
    public float increaseWoodTimeInterval;
    public GameObject woodPrefab;

    private float _elapsedTimeSpawnTime = 0f;
    private float _elapsedTimeWoodQtd = 0f;
    private float _woodTimer;
    private int _woodSpawnQtd = 1;
    // Start is called before the first frame update
    void Start()
    {
        _woodTimer = maxWoodSpawnTime;
        _elapsedTimeSpawnTime = 0f;
        _elapsedTimeWoodQtd = 0f;
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
            if (Time.timeSinceLevelLoad - _elapsedTimeSpawnTime > decreaseTimeInterval)
            {
                UpdateTimeInterval();
                _elapsedTimeSpawnTime = Time.timeSinceLevelLoad;
            }
            if (Time.timeSinceLevelLoad - _elapsedTimeWoodQtd > increaseWoodTimeInterval)
            {
                UpdateWoodQtd();
                _elapsedTimeWoodQtd = Time.timeSinceLevelLoad;
            }
            for (int i = 0; i < _woodSpawnQtd; i++)
            {
                float woodX = UnityEngine.Random.Range(-9f, 9f);
                GameObject tempWood = Instantiate(woodPrefab, new Vector3(woodX, -6, 0), Quaternion.identity);
                float forceX = 0f;
                if (woodX > 0)
                {
                    forceX = UnityEngine.Random.Range(-1f, 0f);
                }
                else
                {
                    forceX = UnityEngine.Random.Range(0f, 1f);
                }
                tempWood.GetComponent<Rigidbody2D>().AddForce((Vector3.up + new Vector3(forceX, 0, 0)) * 600);
            }
            yield return new WaitForSeconds(_woodTimer);
        }
    }
    public void UpdateTimeInterval()
    {
        if (_woodTimer > minWoodSpawnTime)
            _woodTimer -= decreaseRate;
    }
    public void UpdateWoodQtd()
    {
        if (_woodSpawnQtd < maxQtdWood)
            _woodSpawnQtd++;
    }
}
