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
    public GameObject ChooseGameMenu;

    public void ChangeGameMenuScene(GameObject gameID)
    {  
        GameMenuScene.SetActive(true);  
        ChooseGameMenu.SetActive(false);
        CurrentGameID.GetComponent<TMPro.TextMeshProUGUI>().text = gameID.GetComponent<TMPro.TextMeshProUGUI>().text;
       
    }         
}


