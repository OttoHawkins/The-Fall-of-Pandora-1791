using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoScene : MonoBehaviour
{
    public Text companyNameText; // ����� ��� �����������
    public float displayDuration = 10.0f; // ����� ����������� ������
    public float letterDelay = 0.1f; // �������� ����� �������
    private string companyName = "�������� ������\r\n 28 ������� 1791\r\n"; // ����� ����� �������� �����
    private bool textDisplayed = false;

    public Text loadingText; // ����� ��� �������� ��������
    private string loadingMessage = "��������"; // ��������� ��������

    public AudioClip displaySound; // ���� ��� ���������������
    private AudioSource audioSource; // ��������� AudioSource

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // ��������� AudioSource � �������
        loadingText.gameObject.SetActive(true); // ���������� ����� ��������
        StartCoroutine(AnimateText()); // �������� �������� ��� �������� ������
        StartCoroutine(AnimateLoadingText()); // �������� �������� ��������� ��������
    }

    IEnumerator AnimateText()
    {
        // ����������� ������ �� ����� �����
        companyNameText.text = ""; // ������� �����
        foreach (char letter in companyName.ToCharArray())
        {
            companyNameText.text += letter; // ��������� �����
            audioSource.PlayOneShot(displaySound); // ������������� ���� ��� ���������� �����
            yield return new WaitForSeconds(letterDelay); // ���� ����� ��������� ������
        }

        // ���� ��������� �����, ����� ���� ������ ������, ��� ����� ��������� ��������
        yield return new WaitForSeconds(0.5f); // 0.5 ������� �������� ����� ������������

        // ����, ���� ���� ����� ����� ���������
        textDisplayed = true;
        yield return new WaitForSeconds(displayDuration);

        // ������� ������������ ������
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            Color color = companyNameText.color;
            color.a = t; // ������ ��������� �����
            companyNameText.color = color;
            yield return null; // ���� ���� ����
        }

        // ������ ����� �������� � �����
        loadingText.gameObject.SetActive(false);

        // ��������� ��������� �����
        SceneManager.LoadScene("Lvl1"); // ���������, ��� ������ "SampleScene" ������� ��� ����� ��������� �����
    }

    IEnumerator AnimateLoadingText()
    {
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                loadingText.text = loadingMessage + new string('.', i);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
