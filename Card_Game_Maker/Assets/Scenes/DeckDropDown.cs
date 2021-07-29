using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class DeckDropDown : MonoBehaviour
{
    public TMP_Dropdown myDropdown; // Make sure to assign this
    
List<string> numbers = new List<string>() { "124", "244", "544"};


    public void Start()
    {   
        StartCoroutine(RequestDecks("http://127.0.0.1:8000/decks"));
    }

  

    IEnumerator RequestDecks(string uri)
    {Debug.Log("STart");
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        string[] pages = uri.Split('/');
        int page = pages.Length - 1;
Debug.Log(webRequest.responseCode);
        if (webRequest.responseCode == 200)
        {
            
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);

                    //data = webRequest.downloadHandler.text;
                     
                    myDropdown.options.Clear();
                    foreach (string str in numbers)
                    {
                        myDropdown.options.Add(new TMP_Dropdown.OptionData(str));
                    }

                    break;
            }

        }

    }
}
