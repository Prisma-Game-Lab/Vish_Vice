
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int mineQtd;
    private int metalQtd;
    private GridElement[,] gridArray;
    public List<GridElement> mineList;

    public GameGrid(int width, int height, float cellSize, int maxMines, int maxMetal, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        mineQtd = 0;
        metalQtd = 0;

        gridArray = new GridElement[width, height];
        List<GridElement> mineList = new List<GridElement>();

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridElement(this, x, y);
            }
        }

        while ((metalQtd < maxMetal) || (mineQtd < maxMines))//loop para preencher o mapa com minas e metal
        {
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);

            GridElement mapGridObject = gridArray[x, y];
            if (mineQtd < maxMines)
            {
                if (gridArray[x, y].GetElementType() != GridElementType.mine)
                {
                    gridArray[x, y].SetElementType(GridElementType.mine);
                    mineList.Add(gridArray[x, y]);

                    mineQtd++;
                }
            }
            else if (metalQtd < maxMetal)
            {
                if (gridArray[x, y].GetElementType() != GridElementType.metal)
                {
                    gridArray[x, y].SetElementType(GridElementType.metal);

                    metalQtd++;
                }
            }
        }
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector3 GetWorldPosition(GridElement cell)
    {
        return new Vector3(cell.GetX(), cell.GetY()) * cellSize + originPosition;
    }

    public int GetCellX(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
    }

    public int GetCellY(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public bool CheckCell(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return true;
        }

        return false;
    }

    public void SetEmptyCellValue(int x, int y)
    {
        int mineQtd = 0;
        int metalQtd = 0;

        if (gridArray[x, y].GetElementType() != GridElementType.empty || !CheckCell(x, y))
            return;

        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                    if(i>=0 && i < width && j>=0 && j < height)
                {
                    if (gridArray[i, j].GetElementType() == GridElementType.mine)
                        mineQtd++;
                    else if (gridArray[i, j].GetElementType() == GridElementType.metal)
                        metalQtd++;
                }
                    
            }
        }

        gridArray[x, y].SetElementValue(mineQtd + metalQtd);
    }

    public GridElement GetGridElement(int x, int y)
    {
        if (CheckCell(x, y))
            return gridArray[x, y];

        return null;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

}
