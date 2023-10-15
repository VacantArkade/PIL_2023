using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBlade : InteractableObject
{
   // public new float rotSpeed = 0;
    public Transform child;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //child.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
    }
}
