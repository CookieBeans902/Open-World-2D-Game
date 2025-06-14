using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
public class GameInputManager : MonoBehaviour
{
    private InputActions inputActions;

    public static GameInputManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
    public bool GetDashBool()
    {
        bool dash = inputActions.Player.Dash.WasPressedThisFrame();
        return dash;
    }
}