using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class LoginRequests : MonoBehaviour
{
    public GameObject loginFailText;
    public GameObject signUpFailedText;
    public GameObject chooseGameMenu;
    public GameObject logOutButton;
    public GameObject signUp;
    public GameObject login;
    public GameObject title;

    public void SendLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        string UserName = GameObject.Find("UserNameInputLog").GetComponent<TMP_InputField>().text;
        string Password = GameObject.Find("PasswordInputLog").GetComponent<TMP_InputField>().text;
        if(UserName == "" || Password == "") {yield break;}

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", UserName));
        inputForm.Add(new MultipartFormDataSection("password", Password));
        
        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:8000/login", inputForm);
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 200)
        {
            login.SetActive(false);
            chooseGameMenu.SetActive(true);
            logOutButton.SetActive(true);
            title.GetComponent<TMP_Text>().text ="Choose a Game";
        }
        else 
        {
            loginFailText.SetActive(true);
        }
    }

    public void SendSignUp()
    {
        StartCoroutine(SignUp());
    }
    IEnumerator SignUp()
    {
        string UserName = GameObject.Find("UserNameInputSU").GetComponent<TMP_InputField>().text;
        string Password = GameObject.Find("PasswordInputSU").GetComponent<TMP_InputField>().text;
        if(UserName == "" || Password == "") {yield break;}

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", UserName));
        inputForm.Add(new MultipartFormDataSection("password", Password));
 

        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/signup", inputForm);
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 201)
        {
            signUp.SetActive(false);
            chooseGameMenu.SetActive(true);
            logOutButton.SetActive(true);
            title.GetComponent<TMP_Text>().text ="Choose a Game";
        }
        else 
        {
            signUpFailedText.SetActive(true);
        }
    }
}
