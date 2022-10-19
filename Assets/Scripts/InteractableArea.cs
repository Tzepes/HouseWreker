using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private GameObject prop = null;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            if(prop == null)
            {
                prop = other.gameObject;
                player.GetComponent<PlayerInteractions>().GetProp(prop);
                //player.GetComponent<PlayerInteractions>().GetPropMesh(); //get mesh
                //get collider type
                if (player.GetComponent<PlayerInteractions>().ReturnAuthority())
                {
                    other.GetComponent<PropOutline>().enableOutline();
                    player.GetComponent<PlayerInteractions>().GetTriggerStatus(true);
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            if (player.GetComponent<PlayerInteractions>().ReturnAuthority())
            {
                other.GetComponent<PropOutline>().disableOutline();
            }
            player.GetComponent<PlayerInteractions>().GetTriggerStatus(false);
            player.GetComponent<PlayerInteractions>().GetProp(null);
            prop = null;
        }
    }

    public void DestroyProp()
    {
        Destroy(prop);
    }
}
