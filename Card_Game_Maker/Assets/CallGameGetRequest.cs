using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CallGameGetRequest : MonoBehaviour
{       
    public Button yourButton;
    GameGetRequest scripta;

    // Start is called before the first frame update
    void Start()
    {   Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);  
    }
    void TaskOnClick(){
        
        scripta = GameObject.FindGameObjectWithTag("changegamescene").GetComponent<GameGetRequest>();
        scripta.Start();
    }

}
