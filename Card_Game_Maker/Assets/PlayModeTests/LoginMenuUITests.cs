using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.TestTools;
using TMPro;


public class LoginMenuUITests
{

    [OneTimeSetUp]
    public void LoadScene()
    {
        SceneManager.LoadScene("Index");
    }

    [UnityTest]
    public IEnumerator NavigateToLoginAndBack()
    {
        GameObject.Find("LoginMenuButton").GetComponent<Button>().onClick.Invoke();
        yield return null; 

        Assert.True(GameObject.Find("Login").activeSelf);

        GameObject UserInput = GameObject.Find("UserNameInputLog");
        GameObject PassInput = GameObject.Find("PasswordInputLog");
        UserInput.GetComponent<TMP_InputField>().text = "Something";
        PassInput.GetComponent<TMP_InputField>().text = "Password";
        
        GameObject.Find("BackButtonLog").GetComponent<Button>().onClick.Invoke();
        yield return null; 
        
        Assert.True(GameObject.Find("Index").activeSelf);
        Assert.True(UserInput.GetComponent<TMP_InputField>().text == "");
        Assert.True(PassInput.GetComponent<TMP_InputField>().text == "");
    }

    [UnityTest]
    public IEnumerator NavigateToSignInAndBack()
    {
        GameObject.Find("SignUpMenuButton").GetComponent<Button>().onClick.Invoke();
        yield return null; 
        
        Assert.True(GameObject.Find("SignUp").activeSelf);

        GameObject UserInput = GameObject.Find("UserNameInputSU");
        GameObject PassInput = GameObject.Find("PasswordInputSU");
        UserInput.GetComponent<TMP_InputField>().text = "Something";
        PassInput.GetComponent<TMP_InputField>().text = "Password";

        GameObject.Find("BackButtonSU").GetComponent<Button>().onClick.Invoke();
        yield return null; 
        
        Assert.True(GameObject.Find("Index").activeSelf);
        Assert.True(UserInput.GetComponent<TMP_InputField>().text == "");
        Assert.True(PassInput.GetComponent<TMP_InputField>().text == "");
    }
}
