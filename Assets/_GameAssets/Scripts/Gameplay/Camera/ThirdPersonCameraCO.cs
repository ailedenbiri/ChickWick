using UnityEngine;

public class ThirdPersonCameraCO : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orientationTransform;
    [SerializeField] private Transform playerVisualTransform;
    [SerializeField]  private float rotationSpeed;

   private void Update()
    {
        Vector3 viewDireciton = playerTransform.position - new Vector3(transform.position.x,playerTransform.position.y,transform.position.z);

        orientationTransform.forward = viewDireciton.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = orientationTransform.forward * verticalInput + orientationTransform.right * horizontalInput;
        if(inputDirection != Vector3.zero)
        {
            playerVisualTransform.forward = Vector3.Slerp(playerVisualTransform.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
       
    }
}
