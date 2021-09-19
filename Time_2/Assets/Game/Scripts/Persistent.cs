using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    public int quantWood;
    public int quantMetal;
    public int quantConcrete;
    public int currentDay;

    [HideInInspector] public float currentTime = 2;
    [HideInInspector] public float fullDayLength = 2;

    public static Persistent current;

    private void Awake()
    {
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("persistentData");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        if(current==null)
            current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
