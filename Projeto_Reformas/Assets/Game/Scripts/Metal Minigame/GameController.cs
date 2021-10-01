using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController: MonoBehaviour
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

            if (!grid.CheckCell(x, y))
                return;

            if(grid.GetGridElement(x,y).GetElementType() == GridElementType.mine)
            {
                Debug.Log("Mina");
                visualControl.RevealAllMines();
                //terminar jogo
            }
            else if(grid.GetGridElement(x, y).GetElementType() == GridElementType.metal)
            {
                Debug.Log("Metal");
                visualControl.UpdateCell(x, y);
            }
            else
                RevealGridPosition(x,y);  
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

        // verificar se o valor da celula e zero
        if (cell.GetElementType() == GridElementType.empty && cell.GetElementValue()==0)
        {
            // se for zero, revelar vizinhos
            Debug.Log("vazio");
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
                    Debug.Log("comecou a ver vizinhos");
                    if (neighbour.GetElementType() == GridElementType.empty)
                    {
                        neighbour.Reveal();
                        visualControl.UpdateCell(neighbour);
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

}
