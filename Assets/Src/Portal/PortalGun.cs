using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public void Start()
    {
        // fireBluePortal = ctx => TryPlacePortal(ctx, true);
        // fireRedPortal = ctx => TryPlacePortal(ctx, false);
    }

    private void OnEnable()
    {
        Input.GetInstance().Player.Enable();
        Input.GetInstance().Player.Fire1.performed += TryPlacePortal;
        Input.GetInstance().Player.Fire2.performed += TryPlacePortal;
    }
    
    private void OnDisable()
    {
        Input.GetInstance().Player.Fire1.performed -= TryPlacePortal;
        Input.GetInstance().Player.Fire2.performed -= TryPlacePortal;
        Input.GetInstance().Player.Disable();
    }

    // private void Update()
    // {
    //     transform.localRotation = Quaternion.Euler(camera.Pitch, 0f, 0f);
    // }

    private void TryPlacePortal(InputAction.CallbackContext ctx)
    {
        // first check if wall is a "portalable wall" via a raycast
        var cameraRay = new Ray(camera.transform.position, camera.transform.forward); 
        Physics.Raycast(cameraRay, out RaycastHit hit);
        if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Portalable"))
        {
            // create blue/red particle effects
            
            return;
        }

        // if existing portal for this color is up destroy
        
        // calculate angle to make it normal to the surface
        
        // construct rendertexture and connect to camera then hand off to portal.cs
    }
}