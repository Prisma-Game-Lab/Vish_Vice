using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resizeBackground : MonoBehaviour
{

    void Start()
    {
        ResizeSpriteToScreen();
    }

    private void ResizeSpriteToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = new Vector3(1, 1, 1);

        var width = sr.sprite.bounds.size.x;
        var height = sr.sprite.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector2((float)worldScreenWidth / width,(float) worldScreenHeight / height);
    }
}
