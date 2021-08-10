using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CallDeckScene : MonoBehaviour

{       public Button yourButton;
        NewBehaviourScript script;
        public GameObject gameID;
        public GameObject CurrentGameID;
        
        
    
    // Start is called before the first frame update
    void Start()
    {   Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);  
    }

    void TaskOnClick(){
        
        script = GameObject.FindGameObjectWithTag("changegamescene").GetComponent<NewBehaviourScript>();
        script.getGameID(gameID);
    }

}