using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameID : MonoBehaviour
{

    public GameObject Game;

    // Start is called before the first frame update
    void Start()
    {
        Game.GetComponent<TMPro.TextMeshProUGUI>().text = GameObject.FindWithTag("GameID").GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
