using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInteractions : NetworkBehaviour
{
    [SerializeField]
    private GameObject interactableArea;

    private bool propInTrigger = false;
    private bool hasProp = false;
    private Rigidbody RBprop;
    private Transform prop;

    private Camera mainCamera;

    public override void OnStartAuthority()
    {
        //mainCamera = Camera.main;
    }

    public void Update()
    {
        // ADD CHECK FOR PLAYER AUTHORITY / IS LOCAL PLAYER
    
        if (prop == null) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            CmdInteract();
        }

        if (hasProp)
        {
            pickUp();
        }
        else if (!hasProp)
        {
            Drop();
        }
    }

    private void FixedUpdate()
    {
        if(prop != null && hasProp)
        {
            Vector3 direction = interactableArea.transform.position - RBprop.position;
            RBprop.velocity = direction.normalized;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            propInTrigger = true;
            prop = other.transform;
            RBprop = other.gameObject.GetComponent<Rigidbody>();
            other.GetComponent<PropOutline>().enableOutline();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
            propInTrigger = false;
            prop = null;
            RBprop = null;
        }
    }

    public void CmdInteract()
    {
        hasProp = !hasProp;
    }

    //[ClientRpc]
    public void pickUp()
    {
        prop.SetParent(this.transform);
        prop.GetComponent<PropOutline>().disableOutline();
        RBprop.constraints = RigidbodyConstraints.FreezeRotation;
        RBprop.useGravity = false;
    }

    //[ClientRpc]
    public void Drop()
    {
        prop.parent = null;
        RBprop.constraints = RigidbodyConstraints.None;
        RBprop.useGravity = true;
    }
}
