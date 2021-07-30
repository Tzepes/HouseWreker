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
        hasProp = inTrigger;
    }

    public void GetProp(GameObject _prop)
    {
        sceneProp = _prop;
    }

    void OnChangePickup(EquippedProp oldEquippedProp,  EquippedProp newEquippedProp)
    {
        StartCoroutine(ChangeEquipedProp(newEquippedProp));
    }

    IEnumerator ChangeEquipedProp(EquippedProp newEquippedProp)
    {
        while (interactableArea.transform.childCount > 0)
        {
            Destroy(interactableArea.transform.GetChild(0).gameObject);
            yield return null;
        }

        switch (newEquippedProp)
        {
            case EquippedProp.prop:                
                Instantiate(sceneProp, interactableArea.transform);
                break;
        }
    }

    public void Update()
    {
        if(!hasAuthority) { return; }

        if (sceneProp == null) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (hasProp)
        {
            CmdPickup(EquippedProp.prop);
        }
        else if (!hasProp)
        {
            CmdDrop(EquippedProp.nothing);
        }

        hasProp = !hasProp;
    }

    [Command]
    public void CmdPickup(EquippedProp chosenProp)
    {
        equippedProp = chosenProp;
        //prop.transform.SetParent(interactableArea.transform);
        sceneProp.GetComponent<PropOutline>().disableOutline();
        //sceneProp.GetComponent<Prop>().CreatePickUpModel(interactableArea.transform);  ---> it s place might be taken by Instantiate() inside ChangeEquipedProp
        sceneProp.GetComponent<Prop>().PickingUp();
    }

    [Command]
    public void CmdDrop(EquippedProp drop)
    {
        equippedProp = drop;
    }
}
