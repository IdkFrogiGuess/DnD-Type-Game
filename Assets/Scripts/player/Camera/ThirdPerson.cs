using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    private PlayerControls controls;

    private CinemachineCamera cam;
    private CinemachineOrbitalFollow orbital;
    private Vector2 scrollDelta;

    private float targetZoom;
    private float currentZoom;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Camera.MouseZoom.performed += HandleMouseScroll;

        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();

        targetZoom = currentZoom = orbital.Radius;
<<<<<<< HEAD
=======
        controller.PlayerIndex = GetComponentInParent<PlayerInput>().playerIndex;
        

        brain.ChannelMask = (OutputChannels)(1 << GetComponentInParent<PlayerInput>().playerIndex+1);
        cam.OutputChannel = (OutputChannels)(1 << GetComponentInParent<PlayerInput>().playerIndex+1);
>>>>>>> parent of da85e66 (it is not working)
    }

    private void HandleMouseScroll(InputAction.CallbackContext context)
    {
       scrollDelta = context.ReadValue<Vector2>();
        Debug.Log($"Mouse is scrolling. Value: {scrollDelta}"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollDelta.y != 0)
        {
            if(orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minZoom, maxZoom);
                scrollDelta = Vector2.zero;
            }
        }

        float bumperDelta = controls.Camera.GamePadZoom.ReadValue<float>();
        if(bumperDelta != 0) {
            targetZoom = Mathf.Clamp(orbital.Radius - bumperDelta * zoomSpeed, minZoom, maxZoom);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentZoom;
    }
}
