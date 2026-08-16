using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Object : MonoBehaviour
{
    private Rigidbody rb;
    private Collider coll;
    
    public bool isHeld = false;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 4f;
    [SerializeField] private float maxThrowSpeed = 12f;
    private float holdDistance = 4f;
    
    private Vector3 currentHeldPosition = new Vector3();
    private Vector3 lastHeldPosition = new Vector3();
    private Vector3 appliedLinearVelocity = new Vector3();

    private Camera camera;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        coll.enabled = true;
    }

    public void PlayerRequestCarryObject(Camera camera)
    {
        this.camera = camera;
        isHeld = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        lastHeldPosition = camera.transform.position + camera.transform.forward * holdDistance;
    }

    public void PlayerRequestDropObject()
    {
        isHeld = false;
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 throwVelocity = PlayerMovement.Velocity + Vector3.ClampMagnitude(appliedLinearVelocity, maxThrowSpeed);
        // throwVelocity = Vector3.ClampMagnitude(throwVelocity, maxThrowSpeed);
        
        rb.AddForce(rb.mass * (throwVelocity), ForceMode.Impulse);
        rb.AddTorque(CameraControl.AngularVelocity * Mathf.Deg2Rad, ForceMode.Impulse);
        // coll.enabled = true;
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            // check collider how close to a surface if close push the cube closer to the player camera
            // holdDistance as a function of distance away from surface
            
            // Physics.Raycast(rb.position, rb.transform.forward, out RaycastHit hit);
            Debug.DrawRay(rb.position, camera.transform.forward);
            // holdDistance = Math.Min(hit.distance, maxDistance);
            if (camera != null)
            {
                currentHeldPosition = camera.transform.position + camera.transform.forward * holdDistance;
                appliedLinearVelocity = (currentHeldPosition - lastHeldPosition) / Time.fixedDeltaTime;
                rb.position = camera.transform.position + camera.transform.forward * holdDistance;
                rb.rotation = camera.transform.transform.rotation;
                lastHeldPosition = currentHeldPosition;
            }
        }
    }
}
