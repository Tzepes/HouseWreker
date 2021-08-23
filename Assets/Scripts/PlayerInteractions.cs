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
    private bool throwCalled = false;

    RaycastHit hit;

    [SerializeField]
    private GameObject sceneProp;

    [SerializeField]
    private GameObject scenePropPrefab;

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

    public bool ReturnAuthority()
    {
        return hasAuthority;
    }

    void OnChangePickup(EquippedProp oldEquippedProp,  EquippedProp newEquippedProp)
    {
        ChangeEquipedProp(newEquippedProp);
    }

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

    public void Update()
    {
        if (!hasAuthority) { return; }

        if (Input.GetKeyDown(KeyCode.E) && (propInTrigger || hasProp))
        {
            Interact();
        }

        if (Input.GetMouseButtonDown(1) && hasProp)
        {
            CmdThrow();
            Interact();
        }
    }

    public void Interact()
    {
        hasProp = !hasProp;
        
        if (hasProp)
        {
            CmdChangeEquippedProp(EquippedProp.prop);
            CmdPickup();
        }
        else if (!hasProp)
        {
            CmdChangeEquippedProp(EquippedProp.nothing);
            CmdDrop();
        }        
    }

    [Command]
    public void CmdThrow()
    {
        throwCalled = true;
    }

    [Command]
    public void CmdPickup()
    {
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

        NetworkServer.Spawn(newSceneProp);

        Debug.Log(throwCalled);
        if (throwCalled)
        {            
            newSceneProp.GetComponent<Prop>().ThrowObject(this.gameObject.transform);
            newSceneProp = null;
            throwCalled = false;
        }
    }

    /*[Command]
    public void CmdThrow()
    {
        CmdDrop();
        newSceneProp.GetComponent<Prop>().ThrowObject(this.gameObject.transform);
        newSceneProp = null;
        hasProp = !hasProp;
    }*/

    [Command]
    void CmdChangeEquippedProp(EquippedProp selectedProp)
    {
        equippedProp = selectedProp;
    }
}
