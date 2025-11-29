using UnityEngine;
using UnityEngine.InputSystem;

public class GazeCheck : MonoBehaviour
{
    private InputAction m_rClickAction;
    public Transform cameraTransform;
    public Transform playerTransform;
    public BottleManager bottleManagerRef;
    public float checkDistance = 15f;


    private void Awake()
    {
        m_rClickAction = InputSystem.actions.FindAction("RightClick");
    }

    private void OnEnable()
    {
        m_rClickAction.Enable();
    }

    private void OnDisable()
    {
        m_rClickAction.Disable();
    }

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }
    

    void Update()
    {
        RaycastHit hit;
        if(m_rClickAction.WasPressedThisFrame())
        {
            if (Physics.Raycast(playerTransform.position + new Vector3(0, 0.5f, 0), cameraTransform.forward, out hit, checkDistance))
            {
                if (hit.collider.CompareTag("red_liquid"))
                {
                    bottleManagerRef.liquid = "red";    
                    Debug.Log("Bottle manager set to red.");
                }
                else if (hit.collider.CompareTag("blue_liquid"))
                {
                    bottleManagerRef.liquid = "blue";    
                    Debug.Log("Bottle manager set to blue.");
                }
            }
        }
    }
}
