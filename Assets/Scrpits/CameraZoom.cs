using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineCamera))]
public class CinemachineOrbitalZoom : MonoBehaviour
{
    /*public InputActionReference zoomAction;
    public float zoomSpeed = 2f;
    public float minRadius = 2f;
    public float maxRadius = 8f;

    private CinemachineCamera cmCamera;
    private OrbitalFollow orbitalFollow;

    private void Awake()
    {
        cmCamera = GetComponent<CinemachineCamera>();
        orbitalFollow = cmCamera.GetComponent<OrbitalFollow>();
        if (orbitalFollow == null)
        {
            Debug.LogError("OrbitalFollow component missing on CinemachineCamera!");
        }
    }

    private void OnEnable()
    {
        zoomAction.action.Enable();
    }

    private void OnDisable()
    {
        zoomAction.action.Disable();
    }

    private void Update()
    {
        if (orbitalFollow == null || zoomAction == null || zoomAction.action == null)
            return;

        float scroll = zoomAction.action.ReadValue<float>();
        if (Mathf.Abs(scroll) < 0.01f) return;

        // Zmiana promienia orbity (zoom)
        float r = orbitalFollow.Radius;  // get current radius
        r -= scroll * zoomSpeed;
        r = Mathf.Clamp(r, minRadius, maxRadius);
        orbitalFollow.Radius = r;
    }*/
}
