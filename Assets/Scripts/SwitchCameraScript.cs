using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCameraScript : MonoBehaviour
{

    public GameObject theCamera;
    public GameObject whiteCameraPosition;
    public GameObject blackCameraPosition;
    public GameObject backLightWhite;
    public GameObject backLightBlack;
    public GameObject gridPoints;
    public Text buttonText;
    public bool whitePosition;
    bool gridEnabled;
    // Start is called before the first frame update

    private void Start()
    {
        whitePosition = true;
    }

    public void SwitchCamera()
    {
        if(whitePosition == true)
        {
            theCamera.transform.position = blackCameraPosition.transform.position;
            theCamera.transform.rotation = blackCameraPosition.transform.rotation;
            backLightWhite.SetActive(true);
            backLightBlack.SetActive(false);
            whitePosition = false;
        }
        else
        {
            theCamera.transform.position = whiteCameraPosition.transform.position;
            theCamera.transform.rotation = whiteCameraPosition.transform.rotation;
            backLightWhite.SetActive(false);
            backLightBlack.SetActive(true);
            whitePosition = true;
        }
        
    }

    public void TurnOnGridPoints()
    {
        if(gridEnabled == true)
        {
            gridPoints.SetActive(false);
            gridEnabled = false;
            buttonText.text = "Turn On Grid Points";
        }
        else
        {
            gridPoints.SetActive(true);
            gridEnabled = true;
            buttonText.text = "Turn Off Grid Points";
        }
    }

}
