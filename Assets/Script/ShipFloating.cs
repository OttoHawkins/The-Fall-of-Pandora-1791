using UnityEngine;

public class ShipFloating : MonoBehaviour
{
    public Transform[] floatPoints;  // ����� ����������
    public float buoyancyForce = 15f;  // ����������� ���� ����������
    public float waterLevel = 0f;  // ������� ������� ����
    public float waveAmplitude = 1f;  // ������ ����
    public float waveFrequency = 1f;  // ������� ����
    public float damping = 2f;  // ��������� ��� ������������ �����

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 5f;  // ��������, ��� ����� ����������
        rb.linearDamping = 1f;  // ������������� ��� ���������� ���������� ��������
        rb.angularDamping = 1f;  // ������������� ��������
    }

    void FixedUpdate()
    {
        foreach (Transform point in floatPoints)
        {
            float height = GetWaterHeight(point.position);
            if (point.position.y < height)  // ���� ����� ���� ������ ����
            {
                float force = (height - point.position.y) * buoyancyForce;
                rb.AddForceAtPosition(Vector3.up * force - rb.linearVelocity * damping, point.position);
            }
        }
    }

    float GetWaterHeight(Vector3 position)
    {
        return Mathf.Sin(Time.time * waveFrequency + position.x * 0.1f) * waveAmplitude + waterLevel;
    }
}
