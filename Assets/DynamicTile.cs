using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : KinematicObject
{
    public bool IsBeingGrabbed = false;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var vel = Physics2D.gravity * Time.deltaTime;
        //body.position = body.position + Physics2D.gravity * Time.deltaTime;
    }

    
}

