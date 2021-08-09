using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class NewBehaviourScript : MonoBehaviour
{
    SceneSwitcher script;
    public GameObject CurrentGameID;
   

    public void getGameID(GameObject gameID)
    {   
        CurrentGameID.GetComponent<TMPro.TextMeshProUGUI>().text = gameID.GetComponent<TMPro.TextMeshProUGUI>().text;
        script = GameObject.FindGameObjectWithTag("changegamescene").GetComponent<SceneSwitcher>();
        script.GamesToDecks();
    }         
}