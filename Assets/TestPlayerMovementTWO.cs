using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerMovementTWO : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private CharacterController controller = null;
    private float _directionY;
    private float jumpSpeed = 3f;
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
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
    }

    [ClientCallback]
    private void Update()
    {
        Move();

        moveVector = Vector3.zero;

        if (controller.isGrounded == false)
        {
            moveVector += Physics.gravity;
        }
        controller.Move(moveVector * Time.deltaTime);

        if (controller.isGrounded == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                //Debug.Log("Space was pressed");
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
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;
        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * movementSpeed * Time.deltaTime);
    }

    [Client]
    private void Jump()
    {
        moveVector.y = jumpSpeed;
        controller.Move(moveVector.normalized * 6f * Time.deltaTime);
    }
}