using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private float maxInteractDistance = 5f;
    private Object prev;
        
    private void OnEnable()
    {
        Input.GetInstance().Player.Enable();
        Input.GetInstance().Player.Interact.performed += TryInteractWith;
    }

    private void OnDisable()
    {
        Input.GetInstance().Player.Interact.performed -= TryInteractWith;
        Input.GetInstance().Player.Disable();
    }

    private void TryInteractWith(InputAction.CallbackContext ctx)
    {
        if (prev != null && prev.isHeld)
        {
            prev.PlayerRequestDropObject();
            return;
        }
        var ray = new Ray(camera.transform.position, camera.transform.forward);
        Physics.Raycast(ray, out var hit);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Object") && hit.distance < maxInteractDistance)
        {
            prev = hit.collider.gameObject.GetComponent<Object>();
            if (prev != null)
            {
                prev.PlayerRequestCarryObject(camera);
            }
        }
    }
    
}
