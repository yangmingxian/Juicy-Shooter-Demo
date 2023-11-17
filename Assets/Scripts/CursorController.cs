using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] SpriteRenderer spriteRenderer;
    void Start()
    {
        var hotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }

    // TODO: 射击时 鼠标 Cursor 能够 zoom in
    // cursor有限制，除非使用sprite作为crosshair而不是cursor
}
