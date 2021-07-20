using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public CharacterController Controller
    {
        get { return controller; }
    }

    private Vector3 playerVelocity;

    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    //private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    private PlayerInput playerInput;

    private Vector3 forward;
    private Vector3 right;

    [SerializeField]
    private GameObject body;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = new PlayerInput();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.Player.Movement.ReadValue<Vector2>();
        Vector3 rightMovement = right * movementInput.x;
        Vector3 forwardMovement = forward * movementInput.y;
        Vector3 move = rightMovement + forwardMovement;
        controller.Move(move * Time.deltaTime * playerSpeed);

        /*if (move != Vector3.zero)
        {
            body.transform.forward = move;
        }*/

        // Changes the height position of the player..
        /*if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }*/

        
    }
}
