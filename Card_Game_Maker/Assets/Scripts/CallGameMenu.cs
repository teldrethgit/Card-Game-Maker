using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CallGameMenu : MonoBehaviour
{       public Button yourButton;
        GameMenu script;
        public GameObject gameID;
        public GameObject gameName;
        public GameObject gameHealth;
        public GameObject gameTotalHand;
        public GameObject gameStartingHand;
        public GameObject gameDescription;
        public GameObject gamePublish;
       

    // Start is called before the first frame update
    void Start()
    {   Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);  
    }
    void TaskOnClick(){
        
        script = GameObject.FindGameObjectWithTag("changegamescene").GetComponent<GameMenu>();
        script.ChangeGameMenuScene(gameID,gameName,gameHealth,gameTotalHand,gameStartingHand,gameDescription,gamePublish);
    }

}
