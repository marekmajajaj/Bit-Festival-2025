using UnityEngine;
using UnityEngine.InputSystem;

public class GazeCheck : MonoBehaviour
{
    private InputAction m_rClickAction;
    private InputAction m_lClickAction;
    public Transform cameraTransform;
    public Transform playerTransform;
    public BottleManager bottleManagerRef;
    public float checkDistance = 1f;
    public float holdDistance = 15f;
    public Rigidbody heldObjectRb = null;
    public InputActionAsset InputActions;
    public float holdForce = 4f;       // How hard the object is pulled toward the target
    public float holdDamping = 1f;      // Resists movement to prevent jitter
    public float maxHoldDistance = 2f;   // Prevent stretching too far



    private void Awake()
    {
        m_rClickAction = InputSystem.actions.FindAction("RightClick");
        m_lClickAction = InputSystem.actions.FindAction("Click");
    }

    private void OnEnable()
    {
        m_rClickAction.Enable();
        m_lClickAction.Enable();
    }

    private void OnDisable()
    {
        m_rClickAction.Disable();
        m_lClickAction.Disable();
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
                else if (hit.collider.CompareTag("green_liquid"))
                {
                    bottleManagerRef.liquid = "green";    
                    Debug.Log("Bottle manager set to green.");
                }
                else if (hit.collider.CompareTag("green_interactable") && bottleManagerRef.liquid == "green")
                {
                    GameObject gameObj = hit.collider.gameObject;
                    Rigidbody rb = hit.collider.attachedRigidbody;
                    Collider col = hit.collider;

                    if (rb != null)
                    {
                        if(heldObjectRb != null)
                        {
                            heldObjectRb.useGravity = true;
                            heldObjectRb.constraints = RigidbodyConstraints.None;
                            heldObjectRb = null;
                            col.excludeLayers = LayerMask.GetMask("Nothing");
                        }
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                        Debug.Log("Object frozen: " + rb.gameObject.name);
                        gameObj.tag = "Untagged";
                        bottleManagerRef.liquid = null;
                    }
                    else
                    {
                        Debug.LogWarning("Object has no Rigidbody to freeze!");
                    }
                }
                else if (hit.collider.CompareTag("red_interactable") && bottleManagerRef.liquid == "red")
                {
                    GameObject gameObj = hit.collider.gameObject;
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("future"))
                        {
                            MeshRenderer mr = child.GetComponent<MeshRenderer>();
                            if (mr != null)
                                mr.enabled = true;
                            Collider col = child.GetComponent<Collider>();
                            if (col != null)
                                col.enabled = true;

                            break;
                        }
                    }
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("previous"))
                        {
                            MeshRenderer mr = child.GetComponent<MeshRenderer>();
                            if (mr != null)
                                mr.enabled = false;
                            Collider col = child.GetComponent<Collider>();
                            if (col != null)
                                col.enabled = false;

                            break;
                        }
                    }
                }
                else if (hit.collider.CompareTag("red_interactable") && bottleManagerRef.liquid == "blue")
                {
                    GameObject gameObj = hit.collider.gameObject;
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("previous"))
                        {
                            MeshRenderer mr = child.GetComponent<MeshRenderer>();
                            if (mr != null)
                                mr.enabled = true;
                            Collider col = child.GetComponent<Collider>();
                            if (col != null)
                                col.enabled = true;

                            break;
                        }
                    }
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("future"))
                        {
                            MeshRenderer mr = child.GetComponent<MeshRenderer>();
                            if (mr != null)
                                mr.enabled = false;
                            Collider col = child.GetComponent<Collider>();
                            if (col != null)
                                col.enabled = false;

                            break;
                        }
                    }
                }
            }
        }
        if(m_lClickAction.WasPressedThisFrame())
        {

            if (Physics.Raycast(playerTransform.position + new Vector3(0, 0.5f, 0), cameraTransform.forward, out hit, checkDistance))
            {
                if (heldObjectRb == null)
                {

            
                    if (hit.collider.CompareTag("green_interactable") || hit.collider.CompareTag("red_interactable"))
                    {

                
                        heldObjectRb = hit.collider.attachedRigidbody;
                        Collider col = hit.collider;

                        if (heldObjectRb != null)
                        {
                            heldObjectRb.useGravity = false;
                            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;
                            col.excludeLayers = LayerMask.NameToLayer("Player");
                        }
                    }
                }
                else
                {
                    Collider col = hit.collider;
                    heldObjectRb.useGravity = true;
                    heldObjectRb.constraints = RigidbodyConstraints.None;
                    heldObjectRb = null;
                    Debug.Log("Sanity Check");
                    col.excludeLayers = LayerMask.GetMask("Nothing");
                }
            }
        }
        if (heldObjectRb != null)
        {
            Vector3 targetPos = playerTransform.position + new Vector3(0, 0.5f, 0) + cameraTransform.forward * 1.4f;
            Vector3 toTarget = targetPos - heldObjectRb.position;

            if (toTarget.magnitude > maxHoldDistance)
                toTarget = toTarget.normalized * maxHoldDistance;

            Vector3 force = toTarget * holdForce;

            force -= heldObjectRb.linearVelocity * holdDamping;

            heldObjectRb.AddForce(force, ForceMode.Acceleration);
        }
    }
}
