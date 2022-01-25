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
    [HideInInspector]public int[] repetitions;
    [HideInInspector]public float timeToSpawn;
    private GameObject mat;

    [HideInInspector] public bool fell;

    public GameObject[] mats;
    private int originalDir;
    // Start is called before the first frame update
    void Start()
    {
        totalMaterials = 0;
        timeToSpawn = 1.5f;
        originalDir = direction;
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
                if (mats[i].transform.position.x > 11 || mats[i].transform.position.x < -11)
                {
                    repetitions[mats[i].gameObject.GetComponent<MovingMaterial>().type] -= 1;
                    Destroy(mats[i].gameObject);
                    totalMaterials -= 1;
                }

                if (mats[i].transform.position.y < -15)
                {
                    repetitions[mats[i].gameObject.GetComponent<MovingMaterial>().type] -= 1;
                    Destroy(mats[i].gameObject);
                    totalMaterials -= 1;
                    fell = true;

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
                mat.gameObject.GetComponent<MovingMaterial>().speed = movingSpeed;
                mat.gameObject.GetComponent<MovingMaterial>().direction = direction;
                mat.gameObject.GetComponent<MovingMaterial>().type = chosenMat;
                mat.gameObject.GetComponent<MovingMaterial>().treadmill = originalDir;
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
            yield return new WaitForSeconds(timeToSpawn);
        }
    }


}
