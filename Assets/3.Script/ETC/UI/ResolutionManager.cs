using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private void Start()
    {
        SetDefaultResolution();
    }


    public void SetDefaultResolution()
    {
        int defaultWeight = 1920;
        int defaultHeight = 1080;

        Screen.SetResolution(defaultWeight, defaultHeight, true);
    }
}
