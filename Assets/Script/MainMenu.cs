using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject infoPanel;  
    public GameObject mainMenuPanel; 
    public Toggle soundToggle;

    void Start()
    {
        settingsPanel.SetActive(false);
        infoPanel.SetActive(false);
        mainMenuPanel.SetActive(true); 

        soundToggle.isOn = PlayerPrefs.GetInt("Sound", 1) == 1;

       
        AudioListener.volume = soundToggle.isOn ? 1f : 0f;

        soundToggle.onValueChanged.AddListener(delegate { UpdateSoundState(); });
    }

    private void UpdateSoundState()
    {
     


        AudioListener.volume = soundToggle.isOn ? 1f : 0f;

        PlayerPrefs.SetInt("Sound", soundToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("loading");
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OpenInfo()
    {
        mainMenuPanel.SetActive(false);
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {


        Application.Quit();
    }
}
