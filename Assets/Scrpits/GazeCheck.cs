using UnityEngine;
using UnityEngine.InputSystem;

public class GazeCheck : MonoBehaviour
{
    private InputAction m_rClickAction;
    private InputAction m_lClickAction;
    public Transform cameraTransform;
    public Transform playerTransform;
    public BottleManager bottleManagerRef;
    public PlayerSizeCommander sizeCommanderRef;
    public float checkDistance = 1f;
    public float holdDistance = 15f;
    public Rigidbody heldObjectRb = null;
    public Collider heldcollider = null;
    public InputActionAsset InputActions;
    public float holdForce = 4f;       // How hard the object is pulled toward the target
    public float holdDamping = 1f;      // Resists movement to prevent jitter
    public float maxHoldDistance = 2f;   // Prevent stretching too far
    public float holdStart;
    public bool holdingRed;
    public bool holdingBlue;



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

                    if (rb != null)
                    {
                        if(heldObjectRb != null)
                        {
                            heldObjectRb.useGravity = true;
                            heldObjectRb.constraints = RigidbodyConstraints.None;
                            heldObjectRb = null;
                            heldcollider.excludeLayers = LayerMask.GetMask("Nothing");
                            heldcollider = null;
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
                            child.gameObject.SetActive(true);

                            break;
                        }
                    }
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("previous"))
                        {
                            child.gameObject.SetActive(false);

                            break;
                        }
                    }
                    bottleManagerRef.liquid = null;
                }
                else if (hit.collider.CompareTag("red_interactable") && bottleManagerRef.liquid == "blue")
                {
                    GameObject gameObj = hit.collider.gameObject;
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("previous"))
                        {
                            child.gameObject.SetActive(true);

                            break;
                        }
                    }
                    foreach (Transform child in gameObj.GetComponentsInChildren<Transform>(true))
                    {
                        if (child.CompareTag("future"))
                        {
                            child.gameObject.SetActive(false);

                            break;
                        }
                    }
                    bottleManagerRef.liquid = null;
                }
                else if(bottleManagerRef.liquid == "red")
                {
                    holdStart = Time.time;
                    Debug.Log("Started holding red bottle.");
                    holdingRed = true;
                }
                else if(bottleManagerRef.liquid == "blue")
                {
                    holdStart = Time.time;
                    holdingBlue = true;
                }
            }
        }
        if (m_rClickAction.WasReleasedThisFrame())
        {       
            if(holdingBlue)
            {
                holdingBlue = false;
                float held = Time.time - holdStart;

                if (held >= 2f)
                    if(sizeCommanderRef.playerAge < 2)
                        sizeCommanderRef.playerAge =+ 1;
            }
            else if(holdingRed)
            {
                holdingRed = false;
                float held = Time.time - holdStart;

                if (held >= 2f)
                    if(sizeCommanderRef.playerAge > 0)
                        sizeCommanderRef.playerAge =- 1;
            }
        }
        if(m_lClickAction.WasPressedThisFrame())
        {

            if (heldObjectRb == null)
                {
                if (Physics.Raycast(playerTransform.position + new Vector3(0, 0.5f, 0), cameraTransform.forward, out hit, checkDistance))
                    {

                    if (hit.collider.CompareTag("green_interactable") || hit.collider.CompareTag("red_interactable"))
                    {

                
                        heldObjectRb = hit.collider.attachedRigidbody;
                        heldcollider = hit.collider;

                        if (heldObjectRb != null)
                        {
                            heldObjectRb.useGravity = false;
                            heldObjectRb.constraints = RigidbodyConstraints.FreezeRotation;
                            if(hit.collider.CompareTag("green_interactable"))
                                heldcollider.excludeLayers = LayerMask.NameToLayer("Player");
                        }
                    }
                }
            }
            else
            {
                heldObjectRb.useGravity = true;
                heldObjectRb.constraints = RigidbodyConstraints.None;
                heldObjectRb = null;
                Debug.Log("Sanity Check");                    
                heldcollider.excludeLayers = LayerMask.GetMask("Nothing");
                heldcollider = null;
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
