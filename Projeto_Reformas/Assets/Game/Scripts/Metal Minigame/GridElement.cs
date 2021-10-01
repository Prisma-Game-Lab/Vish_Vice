using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridElementType
{
    mine,
    metal,
    empty
}
public class GridElement
{
    
    private GameGrid grid;
    private int x;
    private int y;
    private GridElementType type;
    private string text;
    private int value;
    private bool isRevealed;


    public GridElement(GameGrid grid, int x, int y)
    {
        type = GridElementType.empty;
        this.grid = grid;
        this.x = x;
        this.y = y;
        text = " ";
        isRevealed = false;
        

    }
    public void SetElementType(GridElementType type)
    {
        this.type = type;
        if (type == GridElementType.metal)
            text = "M";
        else
            text = "*";
    }

    public void SetElementText(string text)
    {
        this.text = text;
    }

    public string GetElementText()
    {
        return text;
    }

    public GridElementType GetElementType()
    {
        return type;
    }

    public int GetElementValue()
    {
        return value;
    }

    public void SetElementValue(int value)
    {
        this.value = value;
    }

    public void Reveal()
    {
        isRevealed = true;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

}
