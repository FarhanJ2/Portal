using System;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Rigidbody rb;
    private Collider coll;
    
    public bool isHeld = false;

    private Func<Vector3> pos;
    private Func<Quaternion> rot;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void PlayerRequestCarryObject(Func<Vector3> pos, Func<Quaternion> rot)
    {
        isHeld = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        this.pos = pos;
        this.rot = rot;

    }

    public void PlayerRequestDropObject()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            rb.position = pos.Invoke();
            rb.rotation = rot.Invoke();
        }
    }
}
