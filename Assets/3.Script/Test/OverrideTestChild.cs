using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideTestChild : OverrideTest
{
    protected override void Start()
    {
        base.Start();

        Debug.LogWarning("ÀÚ½Ä");
    }
}
