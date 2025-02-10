using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  
    public Transform alternativeView;  
    public float distance = 10f;  
    public float rotationSpeed = 5f;  
    public float height = 5f;  
    public float minYAngle = 10f;  
    public float maxYAngle = 60f;  

    private float currentX = 0f;  
    private float currentY = 20f;  
    private bool isAlternativeView = false;

    void Update()
    {
        if (target != null)
        {
          

            if (!isAlternativeView)
            {
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
            }

   
            if (Input.GetKeyDown(KeyCode.C) && alternativeView != null)
            {
                isAlternativeView = !isAlternativeView;
            }
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            if (isAlternativeView && alternativeView != null)
            {
           
                transform.position = alternativeView.position;
                transform.rotation = alternativeView.rotation;
            }
            else
            {
        
                Vector3 targetPosition = target.position;
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                Vector3 offset = rotation * new Vector3(0, height, -distance);
                transform.position = targetPosition + offset;
                transform.LookAt(targetPosition);
            }
        }
    }
}
