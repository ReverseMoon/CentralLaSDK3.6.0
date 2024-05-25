
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI; //needs to be added to interact with unity ui components.
using TMPro;
using VRC.SDKBase;
using VRC.Udon;


public class timenowdelay : UdonSharpBehaviour
{

    //https://docs.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo?view=net-5.0#remarks
    [Tooltip("Formatting info in code comments.")]
    public string format = "hh:mm tt";

    [Tooltip("Some ID's in code comments.")]
    public string timeZoneID = string.Empty;
    /*some common timezone ids you can use. 
     * 
     * Pacific Standard Time
     * Mountain Standard Time
     * Central Standard Time
     * Eastern Standard Time
     * GMT Standard Time
     * Central Europe Standard Time
     * Tokyo Standard Time
     * 
     */

    [Space(5)]
    public bool autoupdate = true;

    [Space(5)]
    [Tooltip("Unity UI or TMPro")]
    public MaskableGraphic display;

    private TimeZoneInfo timezone;


    void Start()
    {
        if (timeZoneID == string.Empty)
        {
            timezone = TimeZoneInfo.Local;
        }
        else
        {
            timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
        }
        _QueHoraEs();
    }


    public void _QueHoraEs()
    {

        if (!display)
        {
            return;
        }

        Type type = display.GetType();
        string tempstring = TimeZoneInfo.ConvertTime(DateTime.Now, timezone).ToString(format).Replace("p. m.", "PM").Replace("a. m.", "AM");

        if (type == typeof(Text))
        {
            if (((Text)display).text != tempstring)
            {
                ((Text)display).text = tempstring;
            }
        }
        else if (type == typeof(TextMeshPro))
        {
            if (((TextMeshPro)display).text != tempstring)
            {
                ((TextMeshPro)display).text = tempstring;
            }
        }
        else if (type == typeof(TextMeshProUGUI))
        {
            if (((TextMeshProUGUI)display).text != tempstring)
            {
                ((TextMeshProUGUI)display).text = tempstring;
            }
        }
        SendCustomEventDelayedFrames("_QueHoraEs", 120);

    }

}
