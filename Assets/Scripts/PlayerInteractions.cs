using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public enum EquippedProp : byte
{
    nothing,
    prop
}

public class PlayerInteractions : NetworkBehaviour
{
    [SerializeField]
    private GameObject interactableArea;

    private bool propInTrigger = false;
    private bool hasProp = false;

    [SerializeField]
    private GameObject sceneProp;

    [SyncVar(hook = nameof(OnChangePickup))]
    public EquippedProp equippedProp;

    public void GetTriggerStatus(bool inTrigger)
    {
        propInTrigger = inTrigger;
    }

    public void GetProp(GameObject _prop)
    {
        sceneProp = _prop;
    }

    void OnChangePickup(EquippedProp oldEquippedProp,  EquippedProp newEquippedProp)
    {
        ChangeEquipedProp(newEquippedProp);
    }

    private void ChangeEquipedProp(EquippedProp newEquippedProp)
    {
        interactableArea.GetComponent<InteractableArea>().DestroyProp();
                                                                      
        switch (newEquippedProp)
        {
            case EquippedProp.prop:
                Instantiate(sceneProp.GetComponent<Prop>().PropModel(), interactableArea.transform);
                break;
        }
    }

    public void Update()
    {
        if(!hasAuthority) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            Interact();
        }
    }

    public void Interact()
    {
        hasProp = !hasProp;
        
        if (hasProp)
        {
            CmdPickup(EquippedProp.prop);
        }
        else if (!hasProp)
        {
            CmdDrop(EquippedProp.nothing);
        }        
    }

    [Command]
    public void CmdPickup(EquippedProp chosenProp)
    {
        Debug.Log("Pickup called");
        equippedProp = chosenProp;
        sceneProp.GetComponent<PropOutline>().disableOutline();
        sceneProp.GetComponent<Prop>().PickingUp();
    }

    [Command]
    public void CmdDrop(EquippedProp drop)
    {
        Debug.Log("drop called");
        if (interactableArea.transform.childCount > 0)
            Destroy(interactableArea.transform.GetChild(0).gameObject);
        equippedProp = drop;
    }
}
