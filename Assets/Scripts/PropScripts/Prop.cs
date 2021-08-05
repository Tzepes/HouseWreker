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

    [SyncVar]
    public GameObject Parent;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Parent);

        if (!IsGrounded)
        {
            if (rb.velocity.y < -6f || rb.velocity.x > 6f || rb.velocity.x < -6f || rb.velocity.z > 6f || rb.velocity.z < -6f)
            {
                maxVel = true;
            }
        }

        if (IsGrounded && maxVel)
        {
            Destroy(gameObject);
        }

        if(Parent != null)
        {
            transform.position = Parent.transform.position + new Vector3(1f, 0f, 0f);
            transform.rotation = Parent.transform.rotation;
            rb.isKinematic = true;
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
