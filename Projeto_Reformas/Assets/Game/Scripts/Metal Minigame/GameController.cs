using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController: MonoBehaviour
{

    public Camera cam;
    public GameGrid grid;
    public GameObject GameOverUI;
    public Text metalCountText;
    public Text finalMetalText;
    [Header("Quantidade inicial de metal")]
    public int minQtdMetal;
    [Header("Quantidade maxima de metal")]
    public int maxQtdMetal;
    [Header("Quantidade inicial de colunas")]
    public int minWidth;
    [Header("Quantidade maxima de colunas")]
    public int maxWidth;
    [Header("Quantidade inicial de linhas")]
    public int minHeight;
    [Header("Quantidade maxima de linhas")]
    public int maxHeight;
    [HideInInspector]public int metalQtd;
    [HideInInspector]public bool gameIsOn = false;

    private VisualControl visualControl;

    //private Vector3 originPosition;
    private int height;
    private int width;
    private int revealedCells;
    private int level;

    private void Awake()
    {
        level = Persistent.current.currentMetalGameLevel;
        Debug.Log(level);
        //originPosition = new Vector3(-minWidth/2, -minHeight/2);
        //grid = new GameGrid(minWidth, minHeight, 1f, minQtdMetal, minQtdMetal, originPosition);
        grid = SetGrid(level);
        gameIsOn = true;
        height = minHeight + level;
        width = minWidth + level;
    }
    void Start()
    {
        visualControl = GetComponent<VisualControl>();
        metalQtd = 0;
        revealedCells = 0;
    }

    // Update is called once per frame
    void Update()
    {
        metalCountText.text = "Metal: " + metalQtd.ToString();
        if (Input.GetMouseButtonDown(0) && gameIsOn)
        {
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
            int x = grid.GetCellX(transform.position);
            int y = grid.GetCellY(transform.position);

            if (!grid.CheckCell(x, y))
                return;
            if (grid.GetGridElement(x, y).IsRevealed())
                return;

            if(grid.GetGridElement(x,y).GetElementType() == GridElementType.mine)
            {
                //Debug.Log("Mina");
                visualControl.RevealAllMines();
                Persistent.current.currentMetalGameLevel = 0;
                GameOver();
            }
            else if(grid.GetGridElement(x, y).GetElementType() == GridElementType.metal)
            {
                //Debug.Log("Metal");
                visualControl.UpdateCell(x, y);
                metalQtd++;
                revealedCells++;
                
            }
            else
            {
                RevealGridPosition(x, y);
                
            }

            if (revealedCells == grid.GetCellsTotal())
            {
                if (Persistent.current.currentMetalGameLevel <= 3)
                    Persistent.current.currentMetalGameLevel = level + 1;
                //Debug.Log("Level "+level);
                SceneManager.LoadScene("MetalGame");
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
            // esquerda
            neighbourList.Add(grid.GetGridElement(x - 1, y));
            // esquerda-baixo
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x - 1, y - 1));
            // esquerda-cima
            if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x - 1, y + 1));
        }
        if (x + 1 < width)
        {
            // direita
            neighbourList.Add(grid.GetGridElement(x + 1, y));
            // direita-baixo
            if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x + 1, y - 1));
            // direita-cima
            if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x + 1, y + 1));
        }
        // cima
        if (y - 1 >= 0) neighbourList.Add(grid.GetGridElement(x, y - 1));
        // baixo
        if (y + 1 < height) neighbourList.Add(grid.GetGridElement(x, y + 1));


        return neighbourList;
    }

    private GridElementType RevealGridPosition(int x, int y)
    {
        return RevealGridPosition(grid.GetGridElement(x, y));
    }
    private GridElementType RevealGridPosition(GridElement cell)
    {
        if (cell == null) return GridElementType.empty;
        // Reveal this object
        if(cell.IsRevealed())
                return GridElementType.empty;
        cell.Reveal();
        visualControl.UpdateCell(cell);
        revealedCells++;

        // verificar se o valor da celula e zero
        if (cell.GetElementType() == GridElementType.empty && cell.GetElementValue()==0)
        {
            // se for zero, revelar vizinhos
            // guardar vizinhos ja verificados
            List<GridElement> alreadyCheckedNeighbourList = new List<GridElement>();
            // lista de vizinhos para verificar
            List<GridElement> checkNeighbourList = new List<GridElement>();
            checkNeighbourList.Add(cell);

            // enquanto ainda existirem vizinhos para verificar
            while (checkNeighbourList.Count > 0)
            {
                //pegar o primeiro
                GridElement checkGridElement = checkNeighbourList[0];
                // remover da lista de vizinhos a verificar
                checkNeighbourList.RemoveAt(0);
                //colocar na lista de ja verificados
                alreadyCheckedNeighbourList.Add(checkGridElement);

                // verificar todos os vizinhos
                foreach (GridElement neighbour in GetNeighbourList(checkGridElement))
                {
                    //Debug.Log("comecou a ver vizinhos");
                    if (neighbour.GetElementType() == GridElementType.empty)
                    {
                        if (neighbour.IsRevealed() == false)
                        {
                            neighbour.Reveal();
                            visualControl.UpdateCell(neighbour);
                            revealedCells++;
                        }
                        
                        if (neighbour.GetElementValue() == 0)
                        {
                            // se valor for 0, adicionar a lista de vizinhos a verificar
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

    private void GameOver()
    {
        Time.timeScale = 0f;
        gameIsOn = false;
        GameOverUI.SetActive(true);
        finalMetalText.text = "Você ganhou " + metalQtd + " metais";
    }
    public GameGrid SetGrid(int level)
    {
        float x = -(minWidth + level) / 2;
        float y = -(minHeight + level) / 2;
        Vector3 originPosition = new Vector3(x, y);
        Debug.Log("x:" + x);
        Debug.Log("y:" + y);
        return new GameGrid(minWidth+level, minHeight+level, 1f, minQtdMetal+level, minQtdMetal+level, originPosition);
    }
}
