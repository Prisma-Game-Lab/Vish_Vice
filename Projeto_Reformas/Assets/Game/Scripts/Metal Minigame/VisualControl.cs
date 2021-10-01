using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualControl : MonoBehaviour
{
    public GameObject emptyCell;

    private GameController gameController;
    private TextMeshPro[,] debugTextArray;
    private GameObject[,] cellArray;
    private int height;
    private int width;
    private GameGrid gameGrid;
    private List<GridElement> mineList;

    void Start()
    {
        gameController = GetComponent<GameController>();
        gameGrid = gameController.grid;
        height = gameGrid.GetHeight();
        width = gameGrid.GetWidth();
        debugTextArray = new TextMeshPro[width, height];
        cellArray = new GameObject[width, height];
        mineList = new List<GridElement>();
        if (gameController.gameIsOn)
        {
            ShowGridElement();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
    private void ShowGridElement()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridElement cell = gameGrid.GetGridElement(x, y);
                debugTextArray[x, y] = CreateGridText(cell.GetElementText(), 10, gameGrid.GetWorldPosition(x, y) + new Vector3(gameGrid.GetCellSize(), gameGrid.GetCellSize()) * 0.5f);
                Vector3 position = gameGrid.GetWorldPosition(x, y) + new Vector3(gameGrid.GetCellSize(), gameGrid.GetCellSize()) * 0.5f;
                cellArray[x,y] = Instantiate(emptyCell, position, Quaternion.identity);
                if (cell.GetElementType() == GridElementType.mine)
                    mineList.Add(cell);
                gameGrid.SetEmptyCellValue(x, y);
                //Debug.DrawLine(gameGrid.GetWorldPosition(x, y), gameGrid.GetWorldPosition(x, y + 1), Color.white, 100f);
                //Debug.DrawLine(gameGrid.GetWorldPosition(x, y), gameGrid.GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
       // Debug.DrawLine(gameGrid.GetWorldPosition(0, height), gameGrid.GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(gameGrid.GetWorldPosition(width, 0), gameGrid.GetWorldPosition(width, height), Color.white, 100f);
    }

    public void UpdateCell(int x, int y)
    {
        if(gameGrid.GetGridElement(x, y).GetElementType() == GridElementType.empty)
        {
            int value = gameGrid.GetGridElement(x, y).GetElementValue();
            if (value != 0)
                debugTextArray[x, y].text = value.ToString();
        }
        cellArray[x, y].transform.GetChild(0).gameObject.SetActive(false);

    }

    public void UpdateCell(GridElement cell)
    {
        UpdateCell(cell.GetX(), cell.GetY());
    }

    public void RevealAllMines()
    {
        foreach(GridElement mine in mineList)
        {
            UpdateCell(mine);
        }
    }
    
}

    
