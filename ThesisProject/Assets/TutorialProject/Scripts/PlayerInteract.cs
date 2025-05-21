using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// interface
public interface IInteractable
{
    void Interact();
}
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] float interactRange;

    void Start()
    {
        playerCamera = Camera.main;
    }

    private void OnInteract(InputValue value)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactRange);

        // shoot the ray and see if it hits anything in range
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // check if the hit object is interactable
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
