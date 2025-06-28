using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class GameInputManager : MonoBehaviour {
    public event EventHandler OnTestPerformed;

    public static GameInputManager Instance { get; private set; }
    private InputActions inputActions;
    private float[] time = { 0, 0 };
    private float checkTime;


    private void Awake() {
        Instance = GetComponent<GameInputManager>();
        inputActions = new InputActions();
        checkTime = 0.05f;
    }

    private void OnEnable() {
        inputActions.Player.Enable();
        inputActions.Player.Test.performed += Input_OnTestPerformed;
    }

    private void OnDisable() {
        inputActions.Player.Test.performed -= Input_OnTestPerformed;
        inputActions.Player.Disable();
    }


    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = inputActions.Player.Movement.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public bool IsInteracting() {
        float isInteraacting = inputActions.Player.Interact.ReadValue<float>();
        if (isInteraacting > 0.3f) {
            if (time[0] > checkTime) {
                time[0] = 0;
                return true;
            }
            else {
                time[0] += Time.deltaTime;
                return false;
            }
        }
        else {
            return false;
        }
    }

    public bool GetDashBool()
    {
        bool dash = inputActions.Player.Dash.WasPressedThisFrame();
        return dash;
    }

    private void Input_OnTestPerformed(InputAction.CallbackContext context) {
        OnTestPerformed?.Invoke(this, EventArgs.Empty);
    }

}