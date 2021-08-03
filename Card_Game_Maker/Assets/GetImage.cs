using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//This script solution is based on work that can be found at https://github.com/greggman/getuserimage-unity-webgl

public class GetImage : MonoBehaviour
{

    #if UNITY_WEBGL

        [DllImport("__Internal")]
        private static extern void getImageFromBrowser(string objectName, string callbackFuncName);

    #endif

    static public void GetImageFromUserAsync(string objectName, string callbackFuncName)
    {
        #if UNITY_WEBGL

            getImageFromBrowser(objectName, callbackFuncName);

        #else

            Debug.LogError("Not implemented in this platform");

        #endif
    }
}