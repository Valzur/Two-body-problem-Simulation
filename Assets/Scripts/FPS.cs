using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI FPSText;
    private float delay;
    [SerializeField]
    private float UpdateRate;
    // Update is called once per frame
    void Start()
    {
        Application.targetFrameRate = 10000;
        QualitySettings.vSyncCount = 0;
    }
    void Update()
    {
        delay -= Time.deltaTime;
        if(delay <= 0)
        {
            delay = 1/UpdateRate;
            FPSText.text = "FPS: " + 1/ Time.deltaTime;
        }
    }
}
