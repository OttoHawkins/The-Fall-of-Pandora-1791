using UnityEngine;

public class SailController : MonoBehaviour
{
    public Transform[] sails;  // Массив парусов
    public Rigidbody shipRigidbody;  // Rigidbody корабля
    public Vector3 windDirection = new Vector3(1, 0, 0);  // Направление ветра
    public float maxSailAngle = 45f;  // Максимальный угол наклона паруса
    public float windStrength = 10f;  // Сила ветра

    private float currentSailAngle = 0f;

    void Update()
    {
        if (shipRigidbody.linearVelocity.magnitude < 0.1f) return; // Если корабль не движется, паруса не меняются

        float speed = shipRigidbody.linearVelocity.magnitude; // Получаем текущую скорость корабля

        // Определяем угол между направлением движения корабля и ветром
        Vector3 shipDirection = shipRigidbody.linearVelocity.normalized;
        float angle = Vector3.SignedAngle(shipDirection, windDirection, Vector3.up);

        // Рассчитываем угол поворота парусов
        float targetSailAngle = Mathf.Lerp(0, maxSailAngle, speed / windStrength) * Mathf.Sign(angle);

        // Плавное изменение угла парусов
        currentSailAngle = Mathf.Lerp(currentSailAngle, targetSailAngle, Time.deltaTime * 2f);

        // Применяем угол ко всем парусам
        foreach (Transform sail in sails)
        {
            if (sail != null)
            {
                sail.localRotation = Quaternion.Euler(0, currentSailAngle, 0);
            }
        }
    }
}
