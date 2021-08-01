using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

//This C# script solution is based on work that can be found at https://github.com/greggman/getuserimage-unity-webgl

public class UploadImage : MonoBehaviour
{
	string[] editData = new string[6];
	
    public void OnMouseOver()
    {
            // NOTE: gameObject.name MUST BE UNIQUE!!!!
            GetImage.GetImageFromUserAsync(gameObject.name, "ReceiveImage");
    }

    static string s_dataUrlPrefix = "data:image/png;base64,";
    public void ReceiveImage(string dataUrl)
    {
        if (dataUrl.StartsWith(s_dataUrlPrefix))
        {
            byte[] pngData = System.Convert.FromBase64String(dataUrl.Substring(s_dataUrlPrefix.Length));

            // Create a new Texture (or use some old one?)
            Texture2D tex = new Texture2D(1, 1); // does the size matter?
            if (tex.LoadImage(pngData))
            {
				Image image = GameObject.Find("ImageButton").GetComponent<Image>();
				Sprite spr = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
				image.sprite = spr;
				editData[6] = BitConverter.ToString(pngData);
				Debug.Log(editData[6]);
            }
            else
            {
                Debug.LogError("could not decode image");
            }
        }
        else
        {
            Debug.LogError("Error getting image:" + dataUrl);
        }
    }
}