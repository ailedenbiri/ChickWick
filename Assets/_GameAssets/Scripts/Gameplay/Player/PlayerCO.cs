using UnityEngine;

public class PlayerCO : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;
    [Header("MovementSettings")]
    [SerializeField] private KeyCode movementKey;
    [SerializeField] private float _movementSpeed;


    [Header("JumpSettings")]
    [SerializeField] private bool canJump;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;


    [Header("SlidingSettings")]
    [SerializeField] private KeyCode slideKey;
    [SerializeField] private float slideMultiplier;
    [SerializeField] private float slideDrag;


    [Header("GroundCheckSettings")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDrag;


    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDir;
    private bool isSliding;

    Rigidbody _playerRigidbody;
    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();

    }

    void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey))
        {
            isSliding = true;
        }
        else if (Input.GetKeyDown(movementKey))
        {
            isSliding = false;
        }
        else if (Input.GetKey(jumpKey) && canJump && isGrounded()) 
        {
            canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), jumpCooldown);
        }
    }

    void SetPlayerMovement()
    {
        _movementDir = _orientationTransform.forward * _verticalInput 
            + _orientationTransform.right * _horizontalInput;

        if (isSliding) 
        {
            _playerRigidbody.AddForce(_movementDir.normalized * _movementSpeed * slideMultiplier, ForceMode.Force);
        }
        else
        {
            _playerRigidbody.AddForce(_movementDir.normalized * _movementSpeed, ForceMode.Force);
        }
       
    }

    void SetPlayerDrag()
    {
        if (isSliding)
        {
            _playerRigidbody.linearDamping = slideDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = groundDrag;
        }
    }
    
    void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);

        if(flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed; 
            _playerRigidbody.linearVelocity
                = new Vector3(limitedVelocity.x,_playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }
    }

    void SetPlayerJumping()
    {
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        _playerRigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJumping()
    {
        canJump = true;
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
    }
}

