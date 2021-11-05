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
    public Text lifeText;
    [Header("Quantidade inicial de metal")]
    public int minQtdMetal;
    [Header("Quantidade inicial de colunas")]
    public int minWidth;
    [Header("Quantidade inicial de linhas")]
    public int minHeight;
    [Header("Quantidade maxima de rodadas")]
    public int maxLevels;
    [HideInInspector]public int metalQtd;
    [HideInInspector]public bool gameIsOn = false;
    [Header("Fator de multiplicacao do metal coletado")]
    public int metalMultiplier;
    [Header("Quantidade inicial de vidas")]
    public int lifeQtd;

    private VisualControl visualControl;
    private MinigameMenu minigameMenu;
    private int height;
    private int width;
    private int revealedCells;
    private int level;
    private int bombsOpened = 0;
    private void Awake()
    {
        level = Persistent.current.currentMetalGameLevel;
        grid = SetGrid();
        gameIsOn = true;
        height = minHeight;
        width = minWidth;
    }
    void Start()
    {
        visualControl = GetComponent<VisualControl>();
        minigameMenu = GetComponent<MinigameMenu>();
        metalQtd = 0;
        revealedCells = 0;
        Debug.Log("earnedMetalQtd"+Persistent.current.earnedMetalQtd);
        Debug.Log("metalQtd" + metalQtd);
    }

    // Update is called once per frame
    void Update()
    {
        metalCountText.text = "Metal coletado: " + metalQtd.ToString() + " / " + grid.GetMetalTotal().ToString();
        lifeText.text = "Vidas restantes " + (lifeQtd - bombsOpened).ToString();
        //Tratamento do clique
        if (Input.GetMouseButtonDown(0) && gameIsOn && !minigameMenu.paused)
        {
            transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, 10f));
            int x = grid.GetCellX(transform.position);
            int y = grid.GetCellY(transform.position);

            if (!grid.CheckCell(x, y))//se lugar do clique nao for celula, retorna
                return;
            if (grid.GetGridElement(x, y).IsRevealed())//se for celula ja aberta, retorna
                return;

            if(grid.GetGridElement(x,y).GetElementType() == GridElementType.mine)
            {
                bombsOpened += 1;               
                if(lifeQtd == bombsOpened)
                {
                    visualControl.RevealAllMines();
                    Persistent.current.currentMetalGameLevel = 0;
                    GameOver();
                }
                else
                {
                    visualControl.UpdateCell(x, y);
                }
                
            }
            else if(grid.GetGridElement(x, y).GetElementType() == GridElementType.metal)
            {
                visualControl.UpdateCell(x, y);
                metalQtd++;
                Persistent.current.earnedMetalQtd += metalMultiplier;
                revealedCells++;
                
            }
            else
            {
                RevealGridPosition(x, y);
                
            }

            if (revealedCells == grid.GetCellsTotal() || metalQtd == grid.GetMetalTotal())
            {
                if (Persistent.current.currentMetalGameLevel <= maxLevels)
                {
                    Persistent.current.currentMetalGameLevel = level + 1;
                    //Persistent.current.earnedMetalQtd += metalQtd*metalMultiplier;
                }
                StartCoroutine(waitLevelChange());   
                //SceneManager.LoadScene("MetalGame");
            }
                  
        }
    }

    //Gera lista de vizinhs de uma celula
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
    //Revela conteudo de uma celula
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
        //Persistent.current.earnedMetalQtd += metalQtd * metalMultiplier;
        finalMetalText.text = "Você ganhou " + Persistent.current.earnedMetalQtd + " metais";
    }

    //Cria tabuleiro
    public GameGrid SetGrid()
    {
        float x = -(minWidth*1.2f) / 2;
        float y = -(minHeight*1.2f) / 2;
        Vector3 originPosition = new Vector3(x, y);
        Debug.Log("x:" + x);
        Debug.Log("y:" + y);
        return new GameGrid(minWidth, minHeight, 1.2f, minQtdMetal + level, minQtdMetal + level, originPosition);
    }

    private IEnumerator waitLevelChange()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MetalGame");
    }
}
