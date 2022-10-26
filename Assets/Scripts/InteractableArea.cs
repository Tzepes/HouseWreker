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
        if (other.tag == "Prop" && !player.GetComponent<PlayerInteractions>().HasProp())
        {
            if(prop == null) 
            {
                // make all of this more readable (use and declare variables)
                prop = other.gameObject;
                
                player.GetComponent<PlayerInteractions>().GetProp(prop);
                player.GetComponent<PlayerInteractions>().GetPropMesh(prop.GetComponent<MeshFilter>().mesh); //get mesh
                //get collider type if(mesh colider) else if (box collider)
                if (prop.GetComponent<MeshCollider>())
                {
                    player.GetComponent<PlayerInteractions>().GetMeshCollider(prop.GetComponent<MeshCollider>());
                } else //if (prop.GetComponent<BoxCollider>())
                {
                    player.GetComponent<PlayerInteractions>().GetBoxCollider(prop.GetComponent<BoxCollider>());
                }
                player.GetComponent<PlayerInteractions>().GetPropScale(prop.transform.localScale);
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
