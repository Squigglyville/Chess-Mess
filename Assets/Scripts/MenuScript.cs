using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    
    public GameObject mainMenu;
    public GameObject playOptions;
    public GameObject playOnline;
    public GameObject playVSComputer;
    public GameObject credits;
    public GameObject settings;
    public GameObject secret;
    public Toggle pvpToggle;
    
    

    private void Start()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
        settings.SetActive(false);
        playOptions.SetActive(false);
        playOnline.SetActive(false);
        playVSComputer.SetActive(false);
        PlayerPrefs.SetInt("PowerUps", 1);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("exitgame");
    }

    public void Play()
    {
        mainMenu.SetActive(false);
        playOptions.SetActive(true);
        playOnline.SetActive(false);
        playVSComputer.SetActive(false);
        secret.SetActive(false);
    }

    public void PlayOnline()
    {
        playOnline.SetActive(true);
        playOptions.SetActive(false);
    }

    public void PlayVSComputer()
    {
        playVSComputer.SetActive(true);
        playOptions.SetActive(false);
    }

    public void Secret()
    {
        secret.SetActive(true);
        playOptions.SetActive(false);
    }

    public void Credits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void Return()
    {
        mainMenu.SetActive(true);
        credits.SetActive(false);
        settings.SetActive(false);
        playOptions.SetActive(false);
    }

    public void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void ToggleFlick()
    {
        if (pvpToggle.isOn)
        {
            pvpToggle.GetComponentInChildren<Text>().text = "PowerUps On";
            PlayerPrefs.SetInt("PowerUps", 1);
        }
        else
        {
            pvpToggle.GetComponentInChildren<Text>().text = "PowerUps Off";
            PlayerPrefs.SetInt("PowerUps", 0);
        }
    }

    public void StandardGame_M()
    {
        SceneManager.LoadScene("Standard_M");
    }

    public void SemiRandomised_M()
    {
        SceneManager.LoadScene("Semi_M");
    }

    public void FullyRandomised_M()
    {
        SceneManager.LoadScene("Fully_M");
    }

    public void StandardGame()
    {
        SceneManager.LoadScene("Standard");
    }

    public void SemiRandomised()
    {
        SceneManager.LoadScene("Semi");
    }

    public void FullyRandomised()
    {
        SceneManager.LoadScene("Fully");
    }

    public void Challenge()
    {
        SceneManager.LoadScene("Challenge of the Chess God");
    }





}