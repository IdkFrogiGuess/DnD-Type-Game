using System;
using Unity.Cinemachine;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputHandler : MonoBehaviour, AxisState.IInputAxisProvider
{
   
    public InputActionReference horizontal;
    public InputActionReference vertical;

    private void OnEnable()
    {
        horizontal?.action?.Enable();
        vertical?.action?.Enable();
    }

    private void OnDisable()
    {
        horizontal?.action?.Disable();
        vertical?.action?.Disable();
    }

    public float GetAxisValue(int axis)
    {
        switch (axis)
        {
            case 0: return horizontal?.action?.ReadValue<Vector2>().x ?? 0f;
            case 1: return horizontal?.action?.ReadValue<Vector2>().y ?? 0f;
            case 2: return vertical?.action?.ReadValue<float>() ?? 0f;
        }
        return 0f;
    }
}