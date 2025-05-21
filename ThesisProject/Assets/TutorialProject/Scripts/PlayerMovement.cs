using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody characterRB;
    CapsuleCollider characterCollider;
    Vector3 movementInput;
    Vector3 movementVector;
    [SerializeField] private float movementSpeed = 150;
    [SerializeField] private float sprintMult = 2;
    [SerializeField] private float crouchMult = 0.5f;

    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchHeight = 1;
    private float originalHeight;

    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;

    // Start is called before the first frame update
    void Start()
    {
        characterRB = GetComponent<Rigidbody>();
        characterCollider = GetComponent<CapsuleCollider>();
        originalHeight = characterCollider.height;
    }

    private void OnMovement(InputValue input)
    {
        movementInput = new Vector3(input.Get<Vector2>().x, 0, input.Get<Vector2>().y);
    }

    private void OnMovementStop()
    {
        movementInput = Vector3.zero;
    }

    private void OnJump(InputValue input)
    {
        if (isGrounded)
        {
            characterRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SprintInput();
        CrouchInput();

        float currentSpeed = movementSpeed;
        if (isSprinting && !isCrouching)
        {
            currentSpeed *= sprintMult;
        }
        else if (isCrouching)
        {
            currentSpeed *= crouchMult;
        }

        if (movementInput != Vector3.zero)
        {
            movementVector = (movementInput.x * transform.right) + (movementInput.z * transform.forward);
            movementVector.y = 0; // Ensure no vertical movement

            // apply movement
            Vector3 newVelocity = movementVector * currentSpeed * Time.fixedDeltaTime;
            newVelocity.y = characterRB.velocity.y; // Preserve gravity/jump
            characterRB.velocity = newVelocity;
        }
        else
        {
            // stop horizontal movement but preserve gravity
            characterRB.velocity = new Vector3(0, characterRB.velocity.y, 0);
        }
    }

    private void SprintInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isSprinting = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;
    }

    private void CrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            characterCollider.height = crouchHeight;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
            characterCollider.height = originalHeight;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

}
