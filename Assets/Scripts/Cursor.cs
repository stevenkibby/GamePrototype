using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDefault : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursor;

    void Awake()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
}
