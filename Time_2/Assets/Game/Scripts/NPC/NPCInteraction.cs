using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RelationshipStatus{
    low,
    neutral,
    high
}
public class NPCInteraction : MonoBehaviour
{
    public string npcName;
    public RelationshipStatus npcStatus;
    public List<Dialogue> GreetingOptions;
    
    private void Start()
    {
        //npcStatus = quando o sistema de relacionamentos for implementado, pegar o valor dessa variavel que esta guardado na memoria.
    }

    public Dialogue Greet()
    {
        switch (npcStatus)
        {
            case RelationshipStatus.low:
                return GreetingOptions[0];
            case RelationshipStatus.neutral:
                return GreetingOptions[1];
            default:
                return GreetingOptions[2];
        }
    }
}
