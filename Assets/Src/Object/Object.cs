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
    [SerializeField] private LayerMask obstructionMask;
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
            if (camera != null)
            {
                Vector3 rayOrigin = camera.transform.position;
                Vector3 rayDir = camera.transform.forward;
                float targetDistance = maxDistance;
                if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit, maxDistance, obstructionMask))
                {
                    float objectRadius = coll.bounds.extents.magnitude;
                    targetDistance = hit.distance - objectRadius;
                }
                holdDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
                currentHeldPosition = rayOrigin + rayDir * holdDistance;
                appliedLinearVelocity = (currentHeldPosition - lastHeldPosition) / Time.fixedDeltaTime;
                rb.position = currentHeldPosition;
                rb.rotation = camera.transform.rotation;
                lastHeldPosition = currentHeldPosition;
            }
        }
    }
}


