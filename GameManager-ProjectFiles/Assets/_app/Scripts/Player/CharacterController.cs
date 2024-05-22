using System;
using _app.Scripts;
using _app.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /*
    [Header("Movement Components")]
    public float movementSpeed;
    public Vector3 jumpforce;
    public Vector3 movementVector;
    public bool crouching;

    [Header("Player Components")]
    public Rigidbody rb;
    public Camera playerCamera;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.IncreaseScore();
            }
        }
        // Key Down Events for starting movement
        if(Input.GetKeyDown(KeyCode.W))
        {
            movementVector.y = 1;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            movementVector.y = -1;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            movementVector.x = -1;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            movementVector.x = 1;
        }
        
        // Key Up Events for finishing movement
        if(Input.GetKeyUp(KeyCode.W) && (movementVector.y == 1))
        {
            movementVector.y = 0;
        }
        if(Input.GetKeyUp(KeyCode.S) && (movementVector.y == -1))
        {
            movementVector.y = 0;
        }
        if(Input.GetKeyUp(KeyCode.A) && (movementVector.x == -1))
        {
            movementVector.x = 0;
        }
        if(Input.GetKeyUp(KeyCode.D) && (movementVector.x == 1))
        {
            movementVector.x = 0;
        }

        PlayerMovement(movementVector);
    }

    private void PlayerJump()
    {
        rb.AddForce(jumpforce, ForceMode.Impulse);
    }

    private void PlayerMovement(Vector2 movement)
    {
        transform.Translate(translation:new Vector3(movement.x, y:0, movement.y) * movementSpeed * Time.deltaTime);
    }
    */

    [Header("Movement")] 
    public float moveSpeed;

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Keybinds")] public KeyCode jumpKey = KeyCode.Space;
    
    [Header("Ground Check")] 
    public Transform groundCheck;
    public  float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        //  ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround); 
        
        MyInput();
        SpeedControl();
        
        // handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Guard statement to make sure that game manager isn't missing
            if (GameManager.instance != null)
            {
                GameManager.instance.ChangeScore(1);
            }
            else
            {
                Debug.Log("Game Manager is Missing");
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            
            Jump();
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
