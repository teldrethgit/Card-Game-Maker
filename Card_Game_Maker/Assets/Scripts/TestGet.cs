using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class TestGet : MonoBehaviour
{
public void SendPostRequest()
{
    StartCoroutine(TestPost());
}
IEnumerator TestPost()
{
    UnityWebRequest url = UnityWebRequest.Get("https://osucapstone.herokuapp.com/cards");
    yield return url.SendWebRequest();

    if(url.result != UnityWebRequest.Result.ConnectionError)
    {
        GameObject.Find("OutputText").GetComponent<Text>().text = url.downloadHandler.text;
    }
}

}
