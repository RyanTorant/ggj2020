using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTile : MonoBehaviour
{
    public bool IsBeingGrabbed = false;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
