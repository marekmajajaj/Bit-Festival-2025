using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_jumpAction;

    private Vector2 m_moveAmt;

    private Rigidbody m_rigidbody;

    public bool Falling = false;
    public float WalkSpeed = 5;
    public float RotateSpeed = 5;
    public float JumpSpeed = 5;
    public float offsetCzasu = 0.3f;
    public float JumpTime = 0;
    public float JumpHaja = 5f;
    private void OnEnable()
    {
        m_moveAction.Enable();
        m_lookAction.Enable();
        m_jumpAction.Enable();
    }

    private void OnDisable()
    {
        m_moveAction.Disable();
        m_lookAction.Disable();
        m_jumpAction.Disable();
    }

    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_lookAction = InputSystem.actions.FindAction("Look");
        m_jumpAction = InputSystem.actions.FindAction("Jump");

        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        if (IsGrounded())
        {
            WalkSpeed = 5;
        }
        else
        {
            WalkSpeed = 3f;
        }

        if (m_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }


    }

    private void Jump()
    {
        m_rigidbody.AddForceAtPosition(new Vector3(0, JumpHaja, 0), Vector3.up, ForceMode.Impulse);
    }
    private void FixedUpdate()
    {
        Walking();
        RotatePlayerToCamera();

    }

    private void Walking()
    {
        // kierunek względem kamery
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        // ruch = przód/tył + lewo/prawo
        Vector3 moveDir = camForward * m_moveAmt.y + camRight * m_moveAmt.x;
        moveDir.Normalize();

        // MovePosition ruchu
        m_rigidbody.MovePosition(m_rigidbody.position + moveDir * WalkSpeed * Time.deltaTime);
    }
    private void RotatePlayerToCamera()
    {
        // Bierzemy kierunek, w którym patrzy kamera (poziomo)
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(camForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        float checkDistance = 0.51f;   // jak daleko szukać ziemi w dół
        float offset = 0.5f;         // połowa szerokości obiektu (dla 1x1 ≈ 0.5, ale trochę mniej)

        Vector3 pos = transform.position;

        // wszystkie punkty do raycastów
        Vector3[] points = new Vector3[]
        {
        pos,                                   // środek
        pos + new Vector3( offset, 0,  offset), // front-right
        pos + new Vector3(-offset, 0,  offset), // front-left
        pos + new Vector3( offset, 0, -offset), // back-right
        pos + new Vector3(-offset, 0, -offset), // back-left
        };

        foreach (var p in points)
        {
            if (Physics.Raycast(p, Vector3.down, checkDistance))
                return true;
        }

        return false;
    }

}
