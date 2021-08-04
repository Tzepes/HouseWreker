using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Prop : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    public GameObject propModel;
    [SerializeField]
    public GameObject propPrefab;
    private bool IsGrounded;
    private bool maxVel;

    // Update is called once per frame
    void Update()
    {
        if (!IsGrounded)
        {
            if (rb.velocity.y < -6f)
            {
                maxVel = true;
            }
        }

        if (IsGrounded && maxVel)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        IsGrounded = true;
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        IsGrounded = false;
    }

    public void PickingUp()
    {
        Destroy(this.gameObject);
    }

    public GameObject PropModel()
    {
        return propModel;
    }

    public GameObject PropPrefab()
    {
        return propPrefab;
    }
}
