using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prop")
        {
            player.GetComponent<PlayerInteractions>().GetPropInTrigger(true);
            player.GetComponent<PlayerInteractions>().GetProp(other.transform.gameObject);
            other.GetComponent<PropOutline>().enableOutline();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Prop")
        {
            other.GetComponent<PropOutline>().disableOutline();
            player.GetComponent<PlayerInteractions>().GetPropInTrigger(false);
            player.GetComponent<PlayerInteractions>().GetProp(null);
        }
    }
}
