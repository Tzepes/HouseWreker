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

    private void OnStartAuthority()
    {
        interactableArea.SetActive(true);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && propInTrigger){
            hasProp = !hasProp;
        }

        if (hasProp)
        {
            prop.transform.SetParent(this.transform);
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
            Debug.Log("Prop in trigger");
            //fa contur auriu
            propInTrigger = true;
            prop = other.transform;
            RBprop = other.gameObject.GetComponent<Rigidbody>();
        }

        if (other == null)
        {
            propInTrigger = false;
            prop = null;
            RBprop = null;
        }
    }
}
