using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditCardRequests : MonoBehaviour
{
	public GameObject nameInput;
	public GameObject nameButton;
	public GameObject healthInput;
	public GameObject healthButton;
	public GameObject attackInput;
	public GameObject attackButton;
	public GameObject costInput;
	public GameObject costButton;
	public GameObject imageButton;
	public GameObject errorText;
	
	
    public void editName()
	{
		if (nameInput.GetComponent<TMP_InputField>().text != "")
		{
			;
		}
	}
}
