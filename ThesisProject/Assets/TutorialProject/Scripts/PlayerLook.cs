using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private int mouseSensitivity = 5;
    [SerializeField] private Transform playerCamera;

    private float xRotation;
    private float yRotation;
    private float mouseX;
    private float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnLook(InputValue input)
    {
        Vector2 lookInput = input.Get<Vector2>();
        mouseX = lookInput.x;
        mouseY = lookInput.y;
    }

    // Update is called once per frame
    void Update()
    {
        float scaledMouseX = mouseX * mouseSensitivity * Time.deltaTime;
        float scaledMouseY = mouseY * mouseSensitivity * Time.deltaTime;

        xRotation -= scaledMouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 40f);

        yRotation += scaledMouseX;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerCamera.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
