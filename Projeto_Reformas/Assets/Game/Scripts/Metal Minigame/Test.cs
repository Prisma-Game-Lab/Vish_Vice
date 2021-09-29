using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test: MonoBehaviour
{

    public Camera cam;
    public GameGrid grid;

    private Vector3 originPosition;
    void Start()
    {
        originPosition = new Vector3(-9, -4);
        grid = new GameGrid(18, 8, 1f, 15, 10, originPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
            int x = grid.GetCellX(transform.position);
            int y = grid.GetCellY(transform.position);
            //Debug.Log("x"+ x);
            //Debug.Log("y"+ y);
            if (grid.CheckCell(x, y))
            {
                grid.SetEmptyCellValue(x, y);
                grid.UpdateCell(x, y);
            } 
        }
    }

    

}
