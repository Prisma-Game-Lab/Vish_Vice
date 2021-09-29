using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridElementType
{
    bomb,
    resource,
    empty
}
public class GridElement
{

    public GridElementType cellType;
    public string text;
    public int value;


    public GridElement()
    {
        cellType = GridElementType.empty;
        text = "E";
        value = 0;

    }
    public GridElementType SetValue(bool bomb, bool resource)
    {
        cellType = SetElementType(bomb, resource);

        switch (cellType)
        {
            case GridElementType.bomb:
                text= "B";
                break;      
            case GridElementType.resource:
                text = "M";
                break;
            default:
                text = "0";
                break;

        }
        return cellType;
    }

    private GridElementType SetElementType(bool bomb, bool resource)
    {
        int option = 1;//por padrao a celula sera vazia
        if (bomb && resource)
            option = UnityEngine.Random.Range(0, 10);
        else if (bomb)//resource false
            option = UnityEngine.Random.Range(0, 9);
        else if (resource)//bomb false
            option = UnityEngine.Random.Range(1, 10);

        switch (option)
        {
            case 0:
                return GridElementType.bomb;
            case 9:
                return GridElementType.resource;
            default:
                return GridElementType.empty;
        }

    }


    
}
