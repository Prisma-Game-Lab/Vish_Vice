using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MixConcrete : MonoBehaviour
{
    private int totalMaterials = 0;
    [Header("Quantidade maxima de materiais a por na betoneira")]
    public int maxMaterials;

    [Header("Materiais a serem usados")]
    public GameObject[] materials;

    public SpawnConcrete spawnLeft;
    public SpawnConcrete spawnRight;

    [HideInInspector]public int totalConcrete;
    public int maxVida;

    [Header("Texto de Concreto")]
    public TextMeshProUGUI concText;

    private GameObject[] draftedMaterials;
    private int[] quantDrafted;
    private GameObject mat;
    private int ind = 0;
    // Start is called before the first frame update
    void Start()
    {
        quantDrafted = new int[5];
        for (int i = 0; i < 5; i++)
        {
            quantDrafted[i] = 0;
        }

        draftedMaterials = new GameObject[maxMaterials];
        DraftMaterials();
    }

    // Update is called once per frame
    void Update()
    {
        if (totalMaterials <= 0)
        {
            totalConcrete++;
            DraftMaterials();
        }

        if (ind >= maxMaterials)
        {
            ind = 0;
        }

        concText.text = "Concreto: " + totalConcrete.ToString();
    }

    private void DraftMaterials()
    {
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
                totalMaterials = 0;
                ind = 0;
            }

            Destroy(collision.gameObject);
        }
    }

}
