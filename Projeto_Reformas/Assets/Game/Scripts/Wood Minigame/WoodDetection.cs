using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDetection : MonoBehaviour
{
    public Slice slice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Madeira fora de tela");
        if (collision.gameObject.CompareTag("Wood"))
        {  
            slice.lostCount--;
            slice.lostText.text = "Erros restantes: " + slice.lostCount.ToString();
            if (slice.lostCount == 0)
                slice.EndWoodMinigame();
        }
    }
}
