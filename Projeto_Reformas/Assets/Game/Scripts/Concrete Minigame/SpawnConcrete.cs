using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnConcrete : MonoBehaviour
{
    [Header("Materiais a serem usados")]
    public GameObject[] materials;

    [Header("Direção da esteira (1 - direita; -1 - esquerda)")]
    public int direction;
    public int movingSpeed;

    [HideInInspector]public int totalMaterials;
    private int[] repetitions;
    private GameObject mat;

    public GameObject[] mats;
    // Start is called before the first frame update
    void Start()
    {
        totalMaterials = 0;
        repetitions = new int[5];
        for (int i = 0; i < 5; i++)
        {
            repetitions[i] = 0;
        }

        mats = new GameObject[4];
        StartCoroutine(SpawnMaterials());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (mats[i])
            {
                if (mats[i].transform.position.x > 11 || mats[i].transform.position.x < -11 ||
                    mats[i].transform.position.y < -15)
                {
                    repetitions[mats[i].gameObject.GetComponent<MovingMaterial>().type] -= 1;
                    Destroy(mats[i].gameObject);
                    totalMaterials -= 1;
                }
            }
        }
    }

    private IEnumerator SpawnMaterials()
    {
        while (true)
        {
            if (totalMaterials < 4)
            {
                int chosenMat = Random.Range(0, materials.Length);
                while (repetitions[chosenMat] >= 2)
                {
                    chosenMat = Random.Range(0, materials.Length);
                }

                mat = Instantiate(materials[chosenMat], transform.position + new Vector3(-10f * direction, 0f, 0f), Quaternion.identity);
                mat.gameObject.GetComponent<MovingMaterial>().speed = movingSpeed * direction;
                mat.gameObject.GetComponent<MovingMaterial>().type = chosenMat;
                totalMaterials += 1;
                repetitions[chosenMat] += 1;

                for (int i = 0; i < 4; i++)
                {
                    if (!mats[i])
                    {
                        mats[i] = mat;
                        break;
                    }
                }

            }
            yield return new WaitForSeconds(3);
        }
    }

}
