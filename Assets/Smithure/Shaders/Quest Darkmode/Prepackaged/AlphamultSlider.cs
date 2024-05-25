
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;
using TMPro;
using System;

public class AlphamultSlider : UdonSharpBehaviour
{
    public Slider slider;
    public Material Darkmode;
    public Material videoScreen;
    public TextMeshProUGUI timeText;

    public GameObject[] masterObjectList;
    public Collider[] colliderObjectList;
    public GameObject miniScreen;
    private bool rankEnabled = true;
    private bool collidersEnabled = true;
    private bool miniScreenEnabled = false;
    private DateTime currentTime;

    public void Start()
    {
        videoScreen.renderQueue = 3000;
    }

    public void FixedUpdate()
    {
        currentTime = Networking.GetNetworkDateTime();
        string format = currentTime.ToString("dd-MM-yyyy HH:mm:ss tt");
        timeText.text = format;
    }

    public void ToggleRanks()
    {
        rankEnabled = !rankEnabled;
        foreach(GameObject obj in masterObjectList)
        {
            if(obj != null){
                obj.SetActive(rankEnabled);
            }
        }
    }

    public void ToggleColliders()
    {
        collidersEnabled = !collidersEnabled;
        foreach (Collider obj in colliderObjectList)
        {
            if(obj != null){
                obj.enabled = collidersEnabled;
            }
        }
    }

    public void ToggleMiniScreen()
    {
        miniScreenEnabled = !miniScreenEnabled;
        if(miniScreen != null){
            miniScreen.SetActive(miniScreenEnabled);
        }
    }

    public void SlideUpdate()
    {
        Debug.Log("VAL: " + slider.value);
        Darkmode.SetFloat("_Alpha", slider.value);

    }
}
