using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : KinematicObject
{
    public bool IsBeingGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ComputeVelocity()
    {
        if (!IsBeingGrabbed)
        {
            targetVelocity.y = -9.5f;

        }
        else
        {
            targetVelocity.y = 0.0f;
        }

        

    }
}

