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
    [SerializeField]
    private ScoreDisplay scoreDisplay;

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
            scoreDisplay.SetScore(50);
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

    public void ThrowObject(Transform playerTransform)
    {
        Quaternion rot = Quaternion.Euler(0, playerTransform.transform.eulerAngles.y, 0);

        transform.rotation = rot;

        rb.velocity = transform.forward * 10f;
    }

}
