using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropOutline : MonoBehaviour
{
    [SerializeField]
    private Outline outline;

    public void enableOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 2f;
    }

    public void disableOutline()
    {
        outline.OutlineMode = Outline.Mode.OutlineHidden;
        outline.OutlineWidth = 0f;
    }
}
