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

    public override void OnStartAuthority()
    {
        interactableArea.SetActive(true);
    }

    public void Update()
    {
        if (prop == null) { return; }

        if (Input.GetKeyDown(KeyCode.E) && propInTrigger)
        {
            CmdPickUp();

            if (hasProp)
            {
                pickUp();
            }
            else if (!hasProp)
            {
                Drop();
            }
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
        if(other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
            propInTrigger = false;
            prop = null;
            RBprop = null;
        }
    }

    public void CmdPickUp()
    {
        hasProp = !hasProp;        
    }

    //[ClientRpc]
    public void pickUp()
    {
        prop.transform.SetParent(this.transform);
        prop.GetComponent<PropOutline>().disableOutline();
        RBprop.useGravity = false;
    }

    //[ClientRpc]
    public void Drop()
    {
        prop.transform.parent = null;
        RBprop.useGravity = true;
    }
}
