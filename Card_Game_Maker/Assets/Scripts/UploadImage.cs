using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

//This C# script solution is based on work that can be found at https://github.com/greggman/getuserimage-unity-webgl

public class UploadImage : MonoBehaviour
{
	public string imageData;
	public string base64Data;
	
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
			string b64 = dataUrl.Substring(s_dataUrlPrefix.Length);
			Debug.Log(b64);
            byte[] pngData = System.Convert.FromBase64String(b64);

            Texture2D tex = new Texture2D(1, 1);
            if (tex.LoadImage(pngData))
            {
				Image image = GameObject.Find("ImageButton").GetComponent<Image>();
				Sprite spr = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
				image.sprite = spr;
				imageData = BitConverter.ToString(pngData);
				base64Data = b64;
            }
            else
            {
                Debug.LogError("Error occurred while decoding image");
            }
        }
        else
        {
            Debug.LogError("Error getting image:" + dataUrl);
        }
    }
}