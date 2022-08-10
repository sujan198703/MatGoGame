using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoSettings : MonoBehaviour
{
    private float screenWidth, screenHeight;
    public bool isOptimizeByProcessor, isOptimizeByVRAM, isOptimizeByRam, isOptimizeByVersion;

    [SerializeField] private static bool oneTime = false;
    private int sdk;

    //public int GetSDKLevel()
    //{
    //    var clazz = AndroidJNI.FindClass("android.os.Build$VERSION");
    //    var fieldID = AndroidJNI.GetStaticFieldID(clazz, "SDK_INT", "I");
    //    sdk = AndroidJNI.GetStaticIntField(clazz, fieldID);
    //    return sdk;
    //}

    private void Start()
    {
       // GetSDKLevel();
        //if (!oneTime)
        //{

            AutoSettingsPerDevice();
        //    oneTime = true;
        //}
    }

    public void show_int()
    {
    //    Debug.Log("int");
    //    AdsManager._instance.UnityAdmob(true,false,null);

     //   AdsManager._instance.RequestInterstitial();

    }

    private void SetHeightAndWidthScale()
    {
        screenWidth = (int)(Screen.width);
        screenHeight = (int)(Screen.height);
    }

    public void AutoSettingsPerDevice()
    {
        SetHeightAndWidthScale();


        if (isOptimizeByRam && isOptimizeByProcessor)
            RamAndProcessorOptimization(1000, 1200);
        else if (isOptimizeByRam && isOptimizeByVersion)
            RamAndVersionOptimization(27, 1600 , 2200 , 3000);
        else if (isOptimizeByProcessor)
            ProcessorFrequencyOptimization(1000, 1400, 2400);
        else if (isOptimizeByRam)
            RAMOptimization(1000, 2200, 3000);
        else if (isOptimizeByVRAM)
            VRAMOptimzation(500, 1000, 2000);
      
    }

    private void RAMOptimization(float ramLimit, float ramLimit_2, float ramLimit_3)
    {

        // Ram optimzation
        if (SystemInfo.systemMemorySize < ramLimit)
        {

            Screen.SetResolution((int)(screenWidth * 0.65f), (int)(screenHeight * 0.65f), true);
          //  QualitySettings.SetQualityLevel(0);

        }

        else if (SystemInfo.systemMemorySize < ramLimit_2)
        {
            Screen.SetResolution((int)(screenWidth * 0.7f), (int)(screenHeight * 0.7f), true);
        //    QualitySettings.SetQualityLevel(1);
        }

        else if (SystemInfo.systemMemorySize < ramLimit_3)
        {
            Screen.SetResolution((int)(screenWidth * 0.8f), (int)(screenHeight * 0.8f), true);
        //    QualitySettings.SetQualityLevel(2);
        }
    }

    public void VRAMOptimzation(float vRamLimit, float vRamLimit_2, float vRamLimit_3)
    {


        // VRAM Optimization 
        if (SystemInfo.graphicsMemorySize < vRamLimit)
        {
            Screen.SetResolution((int)(screenWidth * 0.4f), (int)(screenHeight * 0.4f), true);
        }

        else if (SystemInfo.graphicsMemorySize < vRamLimit_2)
        {
            Screen.SetResolution((int)(screenWidth * 0.9f), (int)(screenHeight * 0.9f), true);
        }

        else if (SystemInfo.graphicsMemorySize < vRamLimit_3)
        {
            Screen.SetResolution((int)(screenWidth * 1.0f), (int)(screenHeight * 1.0f), true);
        }
    }

    public void ProcessorFrequencyOptimization(float freqLimit, float freqLimit_2, float freqLimit_3)
    {

        // Processor Optimization 
        if (SystemInfo.processorFrequency < freqLimit)
        {
            Screen.SetResolution((int)(screenWidth * 0.4f), (int)(screenHeight * 0.4f), true);
        }

        else if (SystemInfo.processorFrequency <freqLimit_2)
        {
            Screen.SetResolution((int)(screenWidth * 0.9f), (int)(screenHeight * 0.9f), true);
        }

        else if (SystemInfo.processorFrequency < freqLimit_3)
        {
            Screen.SetResolution((int)(screenWidth * 1.0f), (int)(screenHeight * 1.0f), true);
        }
    }

    public void RamAndProcessorOptimization(float freqLimit, float ramLimit)
    {


        if (SystemInfo.processorFrequency < freqLimit && SystemInfo.systemMemorySize < ramLimit)
        {
            Screen.SetResolution((int)(screenWidth * 0.4f), (int)(screenHeight * 0.4f), true);
        }
        else if (SystemInfo.processorFrequency > SystemInfo.systemMemorySize)
        {
            isOptimizeByRam = false;
            AutoSettingsPerDevice();
        }
        else
        {
            isOptimizeByProcessor = false;
            AutoSettingsPerDevice();
        }
    }

    public void RamAndVersionOptimization(float version, float ramLimit ,float ramlimit2 ,float ramlimit3)
    {


        if ( SystemInfo.systemMemorySize < ramLimit)
        {
          //  Debug.Log("ram0"+sdk);
            Screen.SetResolution((int)(screenWidth * 0.5f), (int)(screenHeight * 0.5f), true);
        }
        else if (SystemInfo.systemMemorySize < ramLimit)
        {
         //   Debug.Log("ram0");

            Screen.SetResolution((int)(screenWidth * 0.6f), (int)(screenHeight * 0.6f), true);
        }
        else if (SystemInfo.systemMemorySize < ramlimit2)
        {
          //  Debug.Log("ram1");

            Screen.SetResolution((int)(screenWidth * 0.7f), (int)(screenHeight * 0.7f), true);
        }
        else if (SystemInfo.systemMemorySize < ramlimit3)
        {
          //  Debug.Log("ram1");

            Screen.SetResolution((int)(screenWidth * 0.75f), (int)(screenHeight * 0.75f), true);
        }
        else
        {
          //  Debug.Log("ram2");

            Screen.SetResolution((int)(screenWidth * 0.8f), (int)(screenHeight * 0.8f), true);
            isOptimizeByVersion = false;
           // AutoSettingsPerDevice();
        }
    }
}
