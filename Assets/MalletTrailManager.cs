using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletTrailManager : MonoBehaviour {

    public GameObject leftMallet;
    public GameObject rightMallet;

    enum triggerStates { standby, enter, hold, release };
    private int rightTriggerState;
    private int leftTriggerState;

    private TrailRenderer tr;

    const float TRIGGER_ENTER_THRESHOLD = 0.35f;

    // Use this for initialization
    void Start () {
        rightTriggerState = (int)triggerStates.standby;
        leftTriggerState = (int)triggerStates.standby;

        tr = rightMallet.AddComponent<TrailRenderer>();
        tr.widthMultiplier = 0.01f;
        tr.time = 0.5f; // fade out time
        tr.enabled = false;
    }

    private int numClicks = 0;

    // Update is called once per frame
    void Update()
    {
        updateRightTriggerState();
        updateLeftTriggerState();


        if (rightTriggerState == (int)triggerStates.enter)
        {
            tr.enabled = true;
        }

        if (rightTriggerState == (int)triggerStates.release)
        {
            tr.enabled = false;
        }
    }

    private void updateRightTriggerState() {
        float rightTriggerVal = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        if (rightTriggerState == (int)triggerStates.standby 
            && rightTriggerVal > 0 && rightTriggerVal < TRIGGER_ENTER_THRESHOLD)
        {
            rightTriggerState = (int)triggerStates.enter;
        }
        else if (rightTriggerVal > TRIGGER_ENTER_THRESHOLD)
        {
            rightTriggerState = (int)triggerStates.hold;
        }
        else if (rightTriggerState == (int)triggerStates.hold 
            && rightTriggerVal < TRIGGER_ENTER_THRESHOLD)
        {
            rightTriggerState = (int)triggerStates.release;
            Debug.Log("release");
        }
        else if (rightTriggerVal == 0)
        {
            rightTriggerState = (int)triggerStates.standby;
        }
    }

    private void updateLeftTriggerState()
    {
        float leftTriggerVal = OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);

        if (leftTriggerState == (int)triggerStates.standby &&
            leftTriggerVal > 0 && leftTriggerVal < TRIGGER_ENTER_THRESHOLD)
        {
            rightTriggerState = (int)triggerStates.enter;
        }
        else if (leftTriggerVal > TRIGGER_ENTER_THRESHOLD)
        {
            leftTriggerState = (int)triggerStates.hold;
        }
        else if (leftTriggerState == (int)triggerStates.hold
            && leftTriggerVal < TRIGGER_ENTER_THRESHOLD)
        {
            leftTriggerState = (int)triggerStates.release;
        }
        else if (leftTriggerVal == 0)
        {
            leftTriggerState = (int)triggerStates.standby;
        }
    }
}
