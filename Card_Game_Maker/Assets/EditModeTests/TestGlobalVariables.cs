using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestGlobalVariables
{
    [Test]
    public void GlobalVariablesAreCreated()
    {
        Assert.True(GlobalManager.GetInstance() is GlobalManager);
        Assert.AreEqual(0, GlobalManager.GetInstance().userId);
    }
}
