using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomLerpSpeed = 10f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    private CinemachineBrain brain;
    private CinemachineCamera cam;
    private CinemachineOrbitalFollow orbital;
    private CinemachineInputAxisController controller;
    private Vector2 scrollDelta;

    private float targetZoom;
    private float currentZoom;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        brain = transform.parent.GetComponentInChildren<CinemachineBrain>();
        cam = GetComponent<CinemachineCamera>();
        orbital = cam.GetComponent<CinemachineOrbitalFollow>();
        controller = cam.GetComponent<CinemachineInputAxisController>();

        targetZoom = currentZoom = orbital.Radius;
        controller.PlayerIndex = GetComponentInParent<PlayerInput>().playerIndex;
        

        brain.ChannelMask = (OutputChannels)(1 << GetComponentInParent<PlayerInput>().playerIndex+1);
        cam.OutputChannel = (OutputChannels)(1 << GetComponentInParent<PlayerInput>().playerIndex+1);
    }

    public void HandleMouseScroll(InputAction.CallbackContext context)
    {
        scrollDelta = context.ReadValue<Vector2>();
        Debug.Log($"Mouse is scrolling. Value: {scrollDelta}");
    }

    public void HandleBumper(InputAction.CallbackContext context)
    {
        targetZoom = Mathf.Clamp(orbital.Radius - context.ReadValue<float>() * zoomSpeed, minZoom, maxZoom);
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollDelta.y != 0)
        {
            if (orbital != null)
            {
                targetZoom = Mathf.Clamp(orbital.Radius - scrollDelta.y * zoomSpeed, minZoom, maxZoom);
                scrollDelta = Vector2.zero;
            }
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
        orbital.Radius = currentZoom;
    }
}
