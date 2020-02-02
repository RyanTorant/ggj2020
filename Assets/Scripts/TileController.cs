using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : KinematicObject
{
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
        targetVelocity.y = -9.5f;

    }
}
