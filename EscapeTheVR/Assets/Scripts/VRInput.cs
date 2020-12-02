using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class VRInput : BaseInput
{
    public Camera eventCamera = null;

    public SteamVR_Action_Boolean triggerButton;
    public SteamVR_Input_Sources handType;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button)
    {
        return triggerButton.GetState(handType);
    }

    public override bool GetMouseButtonDown(int button)
    {
        return triggerButton.GetStateDown(handType);
    }

    public override bool GetMouseButtonUp(int button)
    {
        return triggerButton.GetStateUp(handType);
    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }
}
