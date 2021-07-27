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
    private GameObject sceneObjectPrefab;

    [SyncVar(hook = nameof(OnChangeEquipment))]
    public EquippedItem equippedItem;

    public void GetPropInTrigger(bool inTrigger)
    {
        propInTrigger = inTrigger;
    }

    public void GetProp(GameObject _prop)
    {
        sceneObjectPrefab = _prop;
    }

    public void Update()
    {
        if (sceneObjectPrefab == null) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            Interact();
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

    public void Interact()
    {
        hasProp = !hasProp;
    }

    [Command]
    public void pickUp()
    {
        NetworkServer.Destroy(prop);
        //prop.SetParent(this.transform);
        prop.GetComponent<PropOutline>().disableOutline();
    }

    [Command]
    public void Drop()
    {
        sceneObjectPrefab = null;
    }
}
