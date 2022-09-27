using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class HWPlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private CharacterController controller = null;
    [SerializeField]
    public Transform cam;
    private float _directionY;
    private float jumpSpeed = 4f;
    private float turnSmoothTime = 0.05f;
    private float turnSmoothVelocity;
    private float gravity = 9.81f;

    private bool jumping = false;

    private Vector3 moveVector;

    private Vector2 previousInput;

    private Controls controls;
    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();

        GameObject mainCamObj = GameObject.Find("Main Camera");
        cam = mainCamObj.transform;
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
    }

    [ClientCallback]
    private void Update()
    {
        if(!hasAuthority) { return; }

        Move();

        moveVector = Vector3.zero;

        if (controller.isGrounded == false)
        {
            moveVector += Physics.gravity;
        }
        controller.Move(moveVector * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && controller.isGrounded == true)
        {
            Jump();
            jumping = true;
        }

        if (jumping)
        {
            _directionY -= gravity * Time.deltaTime;

            moveVector.y = _directionY;

            controller.Move(moveVector * movementSpeed * Time.deltaTime);

            if (controller.isGrounded)
            {
                jumping = false;
            }
        }

    }

    [Client]
    private void SetMovement(Vector2 movement)
    {
        previousInput = movement;
    }

    [Client]
    private void ResetMovement()
    {
        previousInput = Vector2.zero;
    }

    [Client]
    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Debug.Log(cam);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }

    [Client]
    private void Jump()
    {
        _directionY = jumpSpeed;
    }
}
