using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDetection : MonoBehaviour
{
    public Slice slice;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wood"))
        {  
            slice.lostCount--;
            slice.lostWoodText.text = "Erros restantes: " + slice.lostCount.ToString();
            if (slice.lostCount == 0)
                slice.EndWoodMinigame();
        }
    }
}
