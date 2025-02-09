using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // Корабль, за которым будет следить камера
    public Transform alternativeView;  // Альтернативная позиция камеры (пустой объект)
    public float distance = 10f;  
    public float rotationSpeed = 5f;  
    public float height = 5f;  
    public float minYAngle = 10f;  
    public float maxYAngle = 60f;  

    private float currentX = 0f;  
    private float currentY = 20f;  
    private bool isAlternativeView = false;  // Флаг переключения камеры

    void Update()
    {
        if (target != null)
        {
            // Управление мышью для вращения камеры (только в стандартном режиме)
            if (!isAlternativeView)
            {
                currentX += Input.GetAxis("Mouse X") * rotationSpeed;
                currentY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
            }

            // Переключение вида по клавише C
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
                // Если включен альтернативный вид, ставим камеру на его позицию
                transform.position = alternativeView.position;
                transform.rotation = alternativeView.rotation;
            }
            else
            {
                // Стандартный вид с возможностью вращения
                Vector3 targetPosition = target.position;
                Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
                Vector3 offset = rotation * new Vector3(0, height, -distance);
                transform.position = targetPosition + offset;
                transform.LookAt(targetPosition);
            }
        }
    }
}
