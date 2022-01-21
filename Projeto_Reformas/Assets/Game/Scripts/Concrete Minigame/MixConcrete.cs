using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MixConcrete : MonoBehaviour
{
    private int totalMaterials = 0;
    [Header("Quantidade maxima de materiais a por na betoneira")]
    public int maxMaterials;

    [Header("Quantidade de concreto ganha ao completar sequencia")]
    public int addConcrete;

    [Header("Aumento de velocidade")]
    public int increaseFactor;
    public int maxSpeed;

    [Header("Materiais a serem usados")]
    public GameObject[] materials;

    public SpawnConcrete spawnLeft;
    public SpawnConcrete spawnRight;

    [HideInInspector]public int totalConcrete;
    public int maxVida;

    [Header("Texto de Concreto")]
    public Text concText;

    [Header("Texto de Vida")]
    public Text lifeText;

    public GameObject[] draftedMaterials;
    public int[] quantDrafted;
    private GameObject mat;
    private int ind = 0;
    private int quantComb;
    private int initialLife;

    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {
        quantComb = 0;
        quantDrafted = new int[5];
        for (int i = 0; i < 5; i++)
        {
            quantDrafted[i] = 0;
        }

        DraftMaterials();

        initialLife = maxVida;
    }

    // Update is called once per frame
    void Update()
    {

        if (ind >= maxMaterials)
        {
            totalConcrete += addConcrete;
            Persistent.current.quantConcrete += addConcrete;
            quantComb++;
            updateSpeed();
            ind = 0;
            this.GetComponents<AudioSource>()[0].Play();
        }

        if (quantComb >= 5)
        {
            maxMaterials++;
            maxMaterials = Mathf.Min(maxMaterials, 6);
            RandomizeDirection();
            quantComb = 0;
        }

        if (totalMaterials <= 0)
        {
            ResetQuantDrafted();
            DraftMaterials();
        }


        concText.text = totalConcrete.ToString();
        lifeText.text = (initialLife - maxVida).ToString() + "/" + initialLife;
    }

    private void DraftMaterials()
    {
        draftedMaterials = new GameObject[maxMaterials];
        int ind2 = 0;
        while (totalMaterials < maxMaterials)
        {
            int matNum = Random.Range(0, materials.Length);
            if (quantDrafted[matNum] < 2)
            {
                mat = Instantiate(materials[matNum], transform.position + new Vector3(-15f + 2*totalMaterials, 0f, 0f),
                    Quaternion.identity);
                mat.gameObject.GetComponent<MovingMaterial>().type = matNum;
                mat.gameObject.GetComponent<MovingMaterial>().isMovingDown = false;
                mat.gameObject.GetComponent<MovingMaterial>().isMovingSide = false;
                mat.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                draftedMaterials[ind2] = mat;
                totalMaterials++;
                ind2++;
                quantDrafted[matNum]++;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            int type = collision.gameObject.GetComponent<MovingMaterial>().type;
            int treadmill = collision.gameObject.GetComponent<MovingMaterial>().treadmill;

            //Atualiza contadores no SpawnConcrete, dependendo da esteira original do material
            if (treadmill > 0)
            {
                spawnLeft.repetitions[type]--;
                spawnLeft.totalMaterials--;

            } else if (treadmill < 0) {

                spawnRight.repetitions[type]--;
                spawnRight.totalMaterials--;

            }

            //Atualiza ordem pedida pela betoneira
            if (type == draftedMaterials[ind].GetComponent<MovingMaterial>().type)
            {
                Destroy(draftedMaterials[ind].gameObject);
                totalMaterials--;
                ind++;

            //Reseta ao errar e diminui a vida em 1
            } else {
                maxVida--;
                if (maxVida == 0)
                {
                    Debug.Log("fim");
                    EndConcrete();
                }
                ResetQuantDrafted();
                for (int i = 0; i < maxMaterials; i++)
                {
                    Destroy(draftedMaterials[i].gameObject);
                }
                totalMaterials = 0;
                ind = 0;
                this.GetComponents<AudioSource>()[1].Play();
            }

            Destroy(collision.gameObject);
        }
    }

    private void ResetQuantDrafted()
    {
        for (int i = 0; i < 5; i++)
        {
            quantDrafted[i] = 0;
        }
    }

    private void EndConcrete()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RandomizeDirection()
    {
        int r1 = Random.Range(0, 2);
        int r2 = Random.Range(0, 2);

        if (r1 == 0) { r1 = -1; }
        if (r2 == 0) { r2 = -1; }

        spawnLeft.direction = r1;
        spawnRight.direction = r2;

        Debug.Log(r1);
        Debug.Log(r2);

        for (int i = 0; i < spawnLeft.mats.Length; i++)
        {
            if (spawnLeft.mats[i])
            {
                spawnLeft.mats[i].GetComponent<MovingMaterial>().direction = r1;
            }
            
            if (spawnRight.mats[i])
            {
                spawnRight.mats[i].GetComponent<MovingMaterial>().direction = r2;
            }
        }
    }

    private void updateSpeed()
    {
        spawnLeft.movingSpeed += increaseFactor;
        spawnRight.movingSpeed += increaseFactor;
        spawnLeft.timeToSpawn -= 0.75f;
        spawnRight.timeToSpawn -= 0.75f;

        spawnLeft.movingSpeed = Mathf.Min(spawnLeft.movingSpeed, maxSpeed);
        spawnRight.movingSpeed = Mathf.Min(spawnRight.movingSpeed, maxSpeed);
        spawnLeft.timeToSpawn = Mathf.Max(spawnLeft.timeToSpawn, 1.5f);
        spawnRight.timeToSpawn = Mathf.Max(spawnRight.timeToSpawn, 1.5f);

        for (int i = 0; i < spawnLeft.mats.Length; i++)
        {
            if (spawnLeft.mats[i])
            {
                spawnLeft.mats[i].GetComponent<MovingMaterial>().speed = spawnLeft.movingSpeed;
            }

            if (spawnRight.mats[i])
            {
                spawnRight.mats[i].GetComponent<MovingMaterial>().speed = spawnRight.movingSpeed;
            }
        }
    }
}
