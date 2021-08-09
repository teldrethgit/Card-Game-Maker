using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameMenu : MonoBehaviour
{
    public GameObject GameMenuScene;
    public GameObject CurrentGameID;
    public GameObject CurrentGameName;
    public GameObject CurrentGameDescription;
    public GameObject CurrentGamePublish;
    public GameObject ChooseGameMenu;
    public GameObject PublishButton;

   

    public void ChangeGameMenuScene(GameObject gameID,GameObject gameName,GameObject gameDescription,GameObject gamePublish )
    {  
        GameMenuScene.SetActive(true);  
        ChooseGameMenu.SetActive(false);
        CurrentGameID.GetComponent<TMPro.TextMeshProUGUI>().text = gameID.GetComponent<TMPro.TextMeshProUGUI>().text;
        CurrentGameName.GetComponent<TMPro.TextMeshProUGUI>().text = gameName.GetComponent<TMPro.TextMeshProUGUI>().text;
        CurrentGameDescription.GetComponent<TMPro.TextMeshProUGUI>().text = gameDescription.GetComponent<TMPro.TextMeshProUGUI>().text;
        CurrentGamePublish.GetComponent<TMPro.TextMeshProUGUI>().text = gamePublish.GetComponent<TMPro.TextMeshProUGUI>().text;
        
        if (CurrentGamePublish.GetComponent<TMPro.TextMeshProUGUI>().text == "True"){
            PublishButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Unpublish Game";
        } else {
            PublishButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Publish Game";
        }
        
    }         
}


