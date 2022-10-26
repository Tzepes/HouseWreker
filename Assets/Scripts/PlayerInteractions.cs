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
    [SerializeField]
    private bool hasProp = false;
    private bool throwCalled = false;

    RaycastHit hit;

    [SerializeField]
    private GameObject sceneProp;
    [SerializeField]
    private Mesh scenePropMesh;
    [SerializeField]
    private BoxCollider scenePropColliderB;
    [SerializeField]
    private MeshCollider scenePropColliderM;
    private Vector3 scenePropScale; 

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

    public void GetPropMesh(Mesh mesh)
    {
        scenePropMesh = mesh;
    }

    public void GetBoxCollider(BoxCollider bColl)
    {
        scenePropColliderB = bColl;
    }

    public void GetMeshCollider(MeshCollider mColl)
    {
        scenePropColliderM = mColl;
    }

    public void GetPropScale(Vector3 pickedPropScale)
    {
        scenePropScale = pickedPropScale;
    }

    public bool HasProp()
    {
        return hasProp;
    }

    private Vector3 CalculateScale(Vector3 propScale) // function currently works only if all x, y, z coords for scale are equal
    {
        float missingPercent = 1 - scenePropScale.x;
        
        Vector3 scaleModifer = propScale * missingPercent;
        propScale = propScale - scaleModifer;
        
        return propScale;
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
            case EquippedProp.prop: //prop is held by player
                if (!sceneProp) { 
                    Debug.Log("PROP NULL!");
                    return;
                }
                // fix prop scale when held here 
                GameObject pickedProp = Instantiate(sceneProp.GetComponent<Prop>().PropModel(), interactableArea.transform.position, interactableArea.transform.rotation);
                pickedProp.transform.SetParent(interactableArea.transform);
                if (pickedProp.transform.localScale != new Vector3(1f, 1f, 1f))
                {
                    pickedProp.transform.localScale = CalculateScale(pickedProp.transform.localScale);
                }
                pickedProp.AddComponent<BoxCollider>();
                break;
            case EquippedProp.nothing: // prop is dropped by player
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

        /*if (!propInTrigger || !hasProp)
        {
            scenePropMesh = null;
            scenePropColliderM = null;
            scenePropColliderB = null;
        }*/
    }

    public void Interact()
    {
        hasProp = !hasProp;
        
        if (!hasProp)
        {
            CmdDrop(EquippedProp.nothing);
        }
        
        if (!sceneProp) { return; }

        if (hasProp)
        {
            CmdPickup(EquippedProp.prop);
        }
    }

    [Command]
    public void CmdThrow()
    {
        throwCalled = true;
    }

    [Command]
    public void CmdPickup(EquippedProp selectedProp)
    {
        equippedProp = selectedProp;        
        sceneProp.GetComponent<PropOutline>().disableOutline();
        sceneProp.GetComponent<Prop>().PickingUp();
    }

    [Command]
    public void CmdDrop(EquippedProp selectedProp)
    {
        equippedProp = selectedProp;

        Vector3 pos = interactableArea.transform.position;
        Quaternion rot = interactableArea.transform.rotation;
        GameObject newSceneProp = Instantiate(scenePropPrefab, pos, rot);

        newSceneProp.GetComponent<Rigidbody>().isKinematic = false;
        newSceneProp.GetComponent<MeshFilter>().mesh = scenePropMesh; //apply mesh
                                                                      //apply collider type
        newSceneProp.GetComponent<MeshCollider>().sharedMesh = scenePropMesh;//apply mesh to collider type

        newSceneProp.transform.localScale = CalculateScale(newSceneProp.transform.localScale);

        NetworkServer.Spawn(newSceneProp);

        if (throwCalled)
        {            
            newSceneProp.GetComponent<Prop>().ThrowObject(this.gameObject.transform);
            newSceneProp = null;
            throwCalled = false;
        }

        scenePropMesh = null;
        scenePropColliderM = null;
        scenePropColliderB = null;
    }
}
