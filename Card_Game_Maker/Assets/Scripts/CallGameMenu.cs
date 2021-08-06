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
       

    // Start is called before the first frame update
    void Start()
    {   Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);  
    }
    void TaskOnClick(){
        
        script = GameObject.FindGameObjectWithTag("changegamescene").GetComponent<GameMenu>();
        script.ChangeGameMenuScene(gameID);
    }

}
