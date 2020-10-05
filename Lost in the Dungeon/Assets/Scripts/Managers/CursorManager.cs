using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager sharedInstance { get; private set; }

    public Texture2D arrowCursor;
    public Texture2D handCursor;
    public Texture2D swordCursor;
    private Vector2 cursorHotspot;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;

        SetCursor(arrowCursor);
    }

    public void SetCursor(Texture2D cursor)
    {
        cursorHotspot = new Vector2(cursor.width / 2, 0.0f);
        Cursor.SetCursor(cursor, cursorHotspot, CursorMode.ForceSoftware);
    }
}
