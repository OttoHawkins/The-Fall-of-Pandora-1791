using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoScene : MonoBehaviour
{
    public Text companyNameText; // Текст для отображения
    public float displayDuration = 10.0f; // Время отображения текста
    public float letterDelay = 0.1f; // Задержка между буквами
    private string companyName = "Торресов пролив\r\n 28 августа 1791\r\n"; // Здесь можно поменять текст
    private bool textDisplayed = false;

    public Text loadingText; // Текст для анимации загрузки
    private string loadingMessage = "Загрузка"; // Сообщение загрузки

    public AudioClip displaySound; // Звук для воспроизведения
    private AudioSource audioSource; // Компонент AudioSource

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource к объекту
        loadingText.gameObject.SetActive(true); // Показываем текст загрузки
        StartCoroutine(AnimateText()); // Начинаем корутину для анимации текста
        StartCoroutine(AnimateLoadingText()); // Начинаем анимацию сообщения загрузки
    }

    IEnumerator AnimateText()
    {
        // Отображение текста по одной букве
        companyNameText.text = ""; // Очищаем текст
        foreach (char letter in companyName.ToCharArray())
        {
            companyNameText.text += letter; // Добавляем букву
            audioSource.PlayOneShot(displaySound); // Воспроизводим звук при добавлении буквы
            yield return new WaitForSeconds(letterDelay); // Ждем перед следующей буквой
        }

        // Ждем некоторое время, чтобы дать понять игроку, что текст полностью отображён
        yield return new WaitForSeconds(0.5f); // 0.5 секунды задержки перед продолжением

        // Ждем, пока весь текст будет отображен
        textDisplayed = true;
        yield return new WaitForSeconds(displayDuration);

        // Плавное исчезновение текста
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            Color color = companyNameText.color;
            color.a = t; // Плавно уменьшаем альфа
            companyNameText.color = color;
            yield return null; // Ждем один кадр
        }

        // Скрыть текст загрузки в конце
        loadingText.gameObject.SetActive(false);

        // Загружаем следующую сцену
        SceneManager.LoadScene("Lvl1"); // Убедитесь, что вместо "SampleScene" указано имя вашей следующей сцены
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
