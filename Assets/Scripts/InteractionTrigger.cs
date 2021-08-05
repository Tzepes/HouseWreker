using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private GameObject prop;
    private bool hasProp = false;

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.E) && !hasProp)
        {
            player.GetComponent<PlayerInteractions>().CmdPickUp(prop);
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasProp)
        {
            player.GetComponent<PlayerInteractions>().CmdDrop();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            player.GetComponent<PlayerInteractions>().GetTrigger(true);
            other.GetComponent<PropOutline>().enableOutline();
            prop = other.gameObject;
            

            //propInTrigger = true;
            //propAux = other.gameObject;

            //prop = propAux.transform;
            //RBprop = propAux.GetComponent<Rigidbody>();

            //propAux.GetComponent<PropOutline>().enableOutline();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            player.GetComponent<PlayerInteractions>().GetTrigger(false);
            other.GetComponent<PropOutline>().disableOutline();
            prop = null;

            //propAux.GetComponent<PropOutline>().disableOutline();
            //propAux = null;
            //propInTrigger = false;

            //prop = null;
            //RBprop = null;
        }
    }

    private void Interact()
    {
        hasProp = !hasProp;
    }
}
