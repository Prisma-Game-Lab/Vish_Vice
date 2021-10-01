using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test: MonoBehaviour
{

    public Camera cam;
    public GameGrid grid;
    [HideInInspector]public bool gameIsOn = false;

    private VisualControl visualControl;

    private Vector3 originPosition;
    private int height = 8;
    private int width = 16;

    private void Awake()
    {
        originPosition = new Vector3(-9, -4);
        grid = new GameGrid(width, height, 1f, 10, 10, originPosition);
        gameIsOn = true;
    }
    void Start()
    {
        visualControl = GetComponent<VisualControl>();
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

            if (grid.CheckCell(x, y))
            {
                RevealGridPosition(x,y);  
            } 
        }
    }
    private List<GridElement> GetNeighbourList(GridElement cell)
    {
        int x = cell.GetX();
        int y = cell.GetY();

        return GetNeighbourList(x, y);
    }
    private List<GridElement> GetNeighbourList(int x, int y)
    {
        List<GridElement> neighbourList = new List<GridElement>();

        if (x - 1 >= 0)
        {
            // Left
            neighbourList.Add(grid.GetGridElement(x - 1, y));
            // Left Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x - 1, y - 1));
            // Left Up
            if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x - 1, y + 1));
        }
        if (x + 1 < width)
        {
            // Right
            neighbourList.Add(grid.GetGridElement(x + 1, y));
            // Right Down
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x + 1, y - 1));
            // Right Up
            if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x + 1, y + 1));
        }
        // Up
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x, y - 1));
        // Down
        if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x, y + 1));


        return neighbourList;
    }

    public GridElementType RevealGridPosition(int x, int y)
    {
        return RevealGridPosition(grid.GetGridElement(x, y));
    }
    public GridElementType RevealGridPosition(GridElement cell)
    {
        if (cell == null) return GridElementType.empty;
        // Reveal this object
        Debug.Log("Aqui");
        if(cell.IsRevealed())
                return GridElementType.empty;
        cell.Reveal();
        visualControl.UpdateCell(cell);

        // Is it an Empty grid object?
        if (cell.GetElementType() == GridElementType.empty && cell.GetElementValue()==0)
        {
            // Is Empty, reveal connected nodes
            Debug.Log("vazio");
            // Keep track of nodes already checked
            List<GridElement> alreadyCheckedNeighbourList = new List<GridElement>();
            // Nodes queued up for checking
            List<GridElement> checkNeighbourList = new List<GridElement>();
            // Start checking this node
            checkNeighbourList.Add(cell);

            // While we have nodes to check
            while (checkNeighbourList.Count > 0)
            {
                // Grab the first one
                GridElement checkGridElement = checkNeighbourList[0];
                // Remove from the queue
                checkNeighbourList.RemoveAt(0);
                alreadyCheckedNeighbourList.Add(checkGridElement);

                // Cycle through all its neighbours
                foreach (GridElement neighbour in GetNeighbourList(checkGridElement))
                {
                    Debug.Log("comecou a ver vizinhos");
                    if (neighbour.GetElementType() == GridElementType.empty)
                    {
                        neighbour.Reveal();
                        visualControl.UpdateCell(neighbour);
                        if (neighbour.GetElementValue() == 0)
                        {
                            // If empty, check add it to queue
                            if (!alreadyCheckedNeighbourList.Contains(neighbour))
                            {
                                checkNeighbourList.Add(neighbour);
                            }
                        }
                    }
                }
            }
        }

        return cell.GetElementType();
    }

}
