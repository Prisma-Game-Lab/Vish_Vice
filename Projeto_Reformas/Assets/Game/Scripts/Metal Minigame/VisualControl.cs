using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualControl : MonoBehaviour
{
    public GameObject emptyCell;
    public TMP_FontAsset font;
    public Sprite metalSprite;
    public Sprite mineSprite;

    private GameController gameController;
    private TextMeshPro[,] textArray;
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
        textArray = new TextMeshPro[width, height];
        cellArray = new GameObject[width, height];
        mineList = new List<GridElement>();
        if (gameController.gameIsOn)
        {
            ShowGrid();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static TextMeshPro CreateGridText(string text, int fontSize, Vector3 worldPosition, TMP_FontAsset font)
    {
        GameObject TextObject = new GameObject("Cell Text", typeof(TextMeshPro));
        TextObject.transform.position = worldPosition;
        TextMeshPro textMeshPro = TextObject.GetComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.font = font;
        textMeshPro.fontSize = fontSize;
        textMeshPro.alignment = TextAlignmentOptions.Midline;

        return textMeshPro;
    }

    //Desenha tabuleiro
    private void ShowGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridElement cell = gameGrid.GetGridElement(x, y);
                textArray[x, y] = CreateGridText(cell.GetElementText(), 11, gameGrid.GetWorldPosition(x, y) + new Vector3(gameGrid.GetCellSize(), gameGrid.GetCellSize()) * 0.5f, font);
                Vector3 position = gameGrid.GetWorldPosition(x, y) + new Vector3(gameGrid.GetCellSize(), gameGrid.GetCellSize()) * 0.5f;
                cellArray[x,y] = Instantiate(emptyCell, position, Quaternion.identity);
                if (cell.GetElementType() == GridElementType.mine)
                {
                    mineList.Add(cell);
                }
                    
                gameGrid.SetEmptyCellValue(x, y);
            }
        }
    }

    //Atualiza valor que sera exibido pela celula quando for aberta
    public void UpdateCell(int x, int y)
    {
        GridElementType type = gameGrid.GetGridElement(x, y).GetElementType();
        if (type == GridElementType.empty)
        {
            GridElement cell = gameGrid.GetGridElement(x, y);
            int value = cell.GetElementValue();
            if (value != 0)
            {
                textArray[x, y].text = value.ToString();
                textArray[x, y].color = cell.GetTextColor();
            }

        }
        else if(type == GridElementType.metal)
        {
            Transform symbol = cellArray[x, y].transform.GetChild(2);
            symbol.GetComponent<SpriteRenderer>().sprite = metalSprite;
            symbol.gameObject.SetActive(true);
        }
        else if(type == GridElementType.mine)
        {
            Transform symbol = cellArray[x, y].transform.GetChild(2);
            symbol.GetComponent<SpriteRenderer>().sprite = mineSprite;
            symbol.gameObject.SetActive(true);
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

    
