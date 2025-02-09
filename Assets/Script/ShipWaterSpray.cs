using UnityEngine;

public class ShipWaterSpray : MonoBehaviour
{
    public ParticleSystem waterSpray; // ������� ������
    public Rigidbody shipRigidbody;   // ������ �������

    void Update()
    {
        if (shipRigidbody == null || waterSpray == null) return;

        float speed = shipRigidbody.linearVelocity.magnitude; // �������� �������� �������
        var emission = waterSpray.emission;

        if (speed > 1f) // ���� ������� ��������
        {
            emission.rateOverTime = speed * 10f; // ��� ���� ��������, ��� ������ �����
            var main = waterSpray.main;
            main.startSpeed = Mathf.Lerp(2f, 6f, speed / 10f); // �������� ������ � ����������� �� ��������
        }
        else
        {
            emission.rateOverTime = 0; // ���� ������� ����� � ��� �����
        }
    }
}
