using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private GameObject prop;

    //PlayerInteractions playerComponent = player.GetComponent<PlayerInteractions>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().enableOutline();
            player.GetComponent<PlayerInteractions>().GetTriggerStatus(true);
            player.GetComponent<PlayerInteractions>().GetProp(other.gameObject);
            prop = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
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
