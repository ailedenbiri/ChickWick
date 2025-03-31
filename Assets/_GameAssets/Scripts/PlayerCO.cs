using UnityEngine;

public class PlayerCO : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Transform _orientationTransform;

    [Header("MovementSettings")]

    [SerializeField] private float _movementSpeed;



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
    }

    void SetPlayerMovement()
    {
        _movementDir = _orientationTransform.forward * _verticalInput 
            + _orientationTransform.right * _horizontalInput;

        _playerRigidbody.AddForce(_movementDir * _movementSpeed, ForceMode.Force);
    }
}
