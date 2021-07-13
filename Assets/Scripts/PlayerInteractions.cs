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

        if(Input.GetKeyDown(KeyCode.E) && propInTrigger){
            hasProp = !hasProp;
        }

        if (hasProp)
        {
            prop.transform.SetParent(this.transform);
            prop.GetComponent<PropOutline>().disableOutline();
            RBprop.useGravity = false;
        }else if (!hasProp)
        {
            prop.transform.parent = null;
            RBprop.useGravity = true;
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

        if (other == null)
        {
            propInTrigger = false;
            prop = null;
            RBprop = null;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
        }
    }
}
