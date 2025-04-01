using UnityEngine;

public class PlayerCO : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform _orientationTransform;

    [Header("MovementSettings")]

    [SerializeField] private float _movementSpeed;

    [Header("JumpSettings")]
    [SerializeField] private bool canJump;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;

    [Header("GroundCheckSettings")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;





   private float _horizontalInput, _verticalInput;
    private Vector3 _movementDir;

    Rigidbody _playerRigidbody;
    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
       
    }

    private void Update()
    {
        SetInputs();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();

    }

    void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && isGrounded()) 
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

        _playerRigidbody.AddForce(_movementDir.normalized * _movementSpeed, ForceMode.Force);
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

