
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameGrid{

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int bombQtd;
    private int resourceQtd;
    private GridElement[,] gridArray;
    private TextMeshPro[,] debugTextArray; 

    public GameGrid(int width, int height, float cellSize, int maxOfBombs, int maxOfResources, Vector3 originPosition){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        bombQtd = 0;
        resourceQtd = 0;

        gridArray = new GridElement[width, height];
        debugTextArray = new TextMeshPro[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = new GridElement();
            }
        }

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                if (x >= 0 && y >= 0 && x < width && y < height)
                {
                    GridElementType type = gridArray[x,y].SetValue(CheckBombQtd(bombQtd, maxOfBombs),CheckResourceQtd(resourceQtd, maxOfResources));
                    if (type == GridElementType.bomb)
                        bombQtd++;
                    else if (type == GridElementType.resource)
                        resourceQtd++;

                }
                debugTextArray[x, y] = CreateGridText(gridArray[x, y].text, 10, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f);

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x+1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

    }


    public static TextMeshPro CreateGridText(string text, int fontSize, Vector3 worldPosition)
    {
        GameObject TextObject = new GameObject("Cell Text", typeof(TextMeshPro));
        TextObject.transform.position = worldPosition;
        TextMeshPro textMeshPro = TextObject.GetComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.fontSize = fontSize;
        textMeshPro.alignment = TextAlignmentOptions.Midline;

        return textMeshPro;
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public int GetCellX(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition - originPosition).x/cellSize);
    }

    public int GetCellY(Vector3 worldPosition)
    {
        return Mathf.FloorToInt((worldPosition- originPosition).y / cellSize);
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
        int bombQtd = 0;
        int resourceQtd = 0;

        if (gridArray[x, y].cellType != GridElementType.empty || !CheckCell(x,y))
            return;

        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (gridArray[i, j].cellType == GridElementType.bomb)
                    bombQtd++;
                else if (gridArray[i, j].cellType == GridElementType.resource)
                    resourceQtd++;
            }
        }

        gridArray[x,y].value =  bombQtd + resourceQtd;
    }

    public void UpdateCell(int x, int y)
    {
        if (gridArray[x, y].cellType != GridElementType.empty || !CheckCell(x, y))
            return;

        debugTextArray[x, y].text = gridArray[x, y].value.ToString();
    }

    private bool CheckBombQtd(int bombQtd, int maxOfBombs)
    {
        if (bombQtd < maxOfBombs)
            return true;
        return false;
    }

    private bool CheckResourceQtd(int resourceQtd, int maxOfResources)
    {
        if (resourceQtd < maxOfResources)
            return true;
        return false;
    }
}
