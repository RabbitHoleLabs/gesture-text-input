using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletTrailManager : MonoBehaviour {

    public GameObject leftMallet;
    public GameObject rightMallet;

    private TrailRenderer tr;

    // Use this for initialization
    void Start () {
        tr = rightMallet.AddComponent<TrailRenderer>();
        tr.widthMultiplier = 0.01f;
        tr.time = 0.5f; // fade out time
        tr.enabled = false;
    }

    private int numClicks = 0;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(TriggerStateManager.rightTriggerState);

        if (TriggerStateManager.rightTriggerState == (int)TriggerStateManager.triggerStates.enter)
        {
            Debug.Log("right trigger enter");
            tr.enabled = true;
        }

        if (TriggerStateManager.rightTriggerState == (int)TriggerStateManager.triggerStates.release)
        {
            Debug.Log("left trigger enter");
            tr.enabled = false;
        }
    }
}
