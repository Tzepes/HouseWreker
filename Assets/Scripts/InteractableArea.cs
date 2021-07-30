using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    //PlayerInteractions playerComponent = player.GetComponent<PlayerInteractions>();

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().enableOutline();
            player.GetComponent<PlayerInteractions>().GetTriggerStatus(true);
            player.GetComponent<PlayerInteractions>().GetProp(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
        }
    }
}
