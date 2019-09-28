using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : Singonton<CursorController>
{
    public bool lockedStart;
    public KeyCode lockKey = KeyCode.Escape;
    public bool active;

    protected override void Awake()
    {
        base.Awake();
        SetCursor();
    }

    private void Update()
    {
        if (!active) return;
        if (Input.GetKeyDown(lockKey))
        {
            lockedStart = !lockedStart;
            SetCursor();
        }
    }

    private void SetCursor()
    {
        if (lockedStart)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
        Cursor.visible = !lockedStart;
    }
}
