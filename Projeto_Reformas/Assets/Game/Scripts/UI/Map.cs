using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public Animator animatior;
    public GameObject mapUI;
    public GameObject playerIcon;
    public GameObject npcs;
    public GameObject player;
    public Sprite exclamatoryQuestIcon;
    public Sprite ponderingQuestIcon;
    public float iconSpeed = 0.3f;

    private Vector3 directionPlayer;
    private Vector3 directionIcon;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = player.GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        directionPlayer = playerInput.direction;
        directionIcon = new Vector3(directionPlayer.x, directionPlayer.z, directionPlayer.y);
        MovePlayerIcon();
    }

    public void OpenMap()
    {
        animatior.Play("MapUI Open");
    }

    public void CloseMap()
    {
        Time.timeScale = 1f;
        animatior.Play("MapUI Close");        
    }

    public void MovePlayerIcon()
    {
        playerIcon.transform.position += directionIcon.normalized * iconSpeed;
    }
}
