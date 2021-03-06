using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridElementType
{
    mine,
    metal,
    empty
}

public enum TextColor
{
    none,
    blue,
    red,
    yellow
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
    private TextColor textColor;
    private bool hasFlag;


    public GridElement(GameGrid grid, int x, int y)
    {
        type = GridElementType.empty;
        this.grid = grid;
        this.x = x;
        this.y = y;
        text = " ";
        isRevealed = false;
        textColor = TextColor.none;
        hasFlag = false;
    }
    public void SetElementType(GridElementType type)
    {
        this.type = type;
        if (type == GridElementType.metal)
            text = "";
        else
            text = "";
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

    public void SetTextColor(TextColor color)
    {
        textColor = color;
    }

    public Color GetTextColor()
    {
        switch (textColor)
        {
            case TextColor.red:
                return new Color(193/255f, 0/255f, 16/255f);
            case TextColor.blue:
                return new Color(0/255f, 108/255f, 255/255f);
            default:
                return Color.yellow;
        }
    }

    public bool IsRevealed()
    {
        return isRevealed;
    }

    public void SetFlag()
    {
        hasFlag = true;
    }

    public void RemoveFlag()
    {
        hasFlag = false;
    }

    public bool IsWithFlag()
    {
        return hasFlag;
    }


}
