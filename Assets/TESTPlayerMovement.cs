using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TESTPlayerMovement : NetworkBehaviour
{
    //[SerializeField]
    //private Rigidbody rb;

    public CharacterController controller;
    public Transform cam;

    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float jumpSpeed = 3f;
    private float _directionY;
    private float gravity = 9.8f;

    public float turnSmoothTime = 0.1f;
    [SerializeField]
    private float turnSmoothVelocity;
    [SyncVar]
    private float angle;
    [SyncVar]
    Vector3 moveDir;


    public override void OnStartClient()
    {
        if(!hasAuthority) { return; }

        controller = GetComponent<CharacterController>();
        controller.detectCollisions = false;

        cam.gameObject.SetActive(true);
        cam = Camera.main.transform;
    }

    [ClientCallback]
    void Update()
    {
        if(!hasAuthority) { return; }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (controller.isGrounded == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                CmdJump();
                Debug.Log("Space was pressed");
            }
        }

        CmdCalculateMovement(horizontal, vertical);

        if (controller.isGrounded == false)
        {
            _directionY -= gravity * Time.deltaTime;
            moveDir.y = _directionY;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

    }

    [Command]
    private void CmdMovementInput(float horizontal, float vertical)
    {

    }

    [Command]
    private void CmdCalculateMovement(float horizontal, float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    [Command]
    private void CmdJump()
    {
        _directionY = jumpSpeed;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }
}
