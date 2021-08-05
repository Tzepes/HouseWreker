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
    private bool propInTrigger = false;
    private bool hasProp = false;
    private Rigidbody RBprop;
    [SerializeField]
    private GameObject sceneProp;

    [SerializeField]
    private GameObject scenePropPrefab;

    [SyncVar(hook = nameof(OnChangePickup))]
    public EquippedProp equippedProp;

    public void GetTriggerStatus(bool inTrigger)

        propInTrigger = inTrigger;
    }
        //mainCamera = Camera.main;
    }

        sceneProp = _prop;
    }

    void OnChangePickup(EquippedProp oldEquippedProp,  EquippedProp newEquippedProp)
    {
        ChangeEquipedProp(newEquippedProp);
    }
        {
    private void ChangeEquipedProp(EquippedProp newEquippedProp)
    {
        switch (newEquippedProp)
        {
            case EquippedProp.prop:
                Instantiate(sceneProp.GetComponent<Prop>().PropModel(), interactableArea.transform);
                break;
            case EquippedProp.nothing:
                if (interactableArea.transform.childCount > 0)
                    Destroy(interactableArea.transform.GetChild(0).gameObject);
                break;
        }
    }
            Drop();
    public void Update()
    {
        if(!hasAuthority) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            Interact();
        }
    }
            RBprop.velocity = direction.normalized;
    public void Interact()
    {
        hasProp = !hasProp;
        
        if (hasProp)
        {
            CmdPickup();
            CmdChangeEquippedProp(EquippedProp.prop);
        }
        else if (!hasProp)
        {
            CmdDrop();
            CmdChangeEquippedProp(EquippedProp.nothing);
        }        
    }

    [Command]
    public void CmdPickup()
    {
        equippedProp = EquippedProp.prop;
        sceneProp.GetComponent<PropOutline>().disableOutline();
        sceneProp.GetComponent<Prop>().PickingUp();
    }

    [Command]
    public void CmdDrop()
    {
        Vector3 pos = interactableArea.transform.position;
        Quaternion rot = interactableArea.transform.rotation;
        GameObject newSceneProp = Instantiate(scenePropPrefab, pos, rot);

        newSceneProp.GetComponent<Rigidbody>().isKinematic = false;

        equippedProp = EquippedProp.nothing;

        NetworkServer.Spawn(newSceneProp);
    }
    {
    [Command]
    void CmdChangeEquippedProp(EquippedProp selectedProp)
    {
        equippedProp = selectedProp;
    }

    //[ClientRpc]
    public void Drop()
    {
        prop.parent = null;
        RBprop.constraints = RigidbodyConstraints.None;
        RBprop.useGravity = true;
    }
}
