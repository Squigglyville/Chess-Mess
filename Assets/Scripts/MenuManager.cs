using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
