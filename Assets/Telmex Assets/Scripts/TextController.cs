
//using AvatarImageReader;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;


public class TextController : UdonSharpBehaviour
{
    [SerializeField]
    RectTransform spacePanel;
    [SerializeField]
    //RuntimeDecoder Decoder;
    //[SerializeField]
    Text PanelPatreons;
    [SerializeField]
    Text receta;
    [Space(10)]
    public int MaxSaltos=2;
    public int MinEspacios = 25;
    public int MaxEspacios=30;
    public float speed = .5f;

    string FormaStr;
    string[] patreons;

    public float PosInicio = 0;
    public float PosEnd=-4000f;
    float PosEndOriginal;


    bool StartPerpetual = false;

    string espacios()
    {
        string espacios_tot = "";
        for (int j=0;j<Random.Range(MinEspacios, MaxEspacios); j++)
        {
            espacios_tot = " " + espacios_tot;
        }
        return espacios_tot;
    }
    string saltos()
    {
        string saltos_tot = "";
        for (int j = 0; j < Random.Range(0, MaxSaltos); j++)
        {
            saltos_tot = "\n" + saltos_tot;
        }
        return saltos_tot;
    }
    public void Start()
    {

        _GetPatreons();
    }
    public void _GetPatreons()
    {
        patreons = receta.text.Split('\n');
        //patreons = Decoder.outputString.Split('\n');

        for (int i=0;i< patreons.Length; i++)
        {
            FormaStr = espacios()+patreons[i]+ saltos()+FormaStr;
        }
        PanelPatreons.text = FormaStr;
        PosEndOriginal = (-PanelPatreons.rectTransform.sizeDelta.y);
        StartPerpetual = true;
    }

    private void Update()
    {
        if (StartPerpetual && PanelPatreons.rectTransform.sizeDelta.y != PosEndOriginal)
        {
            if (spacePanel.localPosition.y > PosEnd)
            {
                spacePanel.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else
            {
                spacePanel.localPosition = Vector3.down * PosInicio;
            }
        }
        else
        {
            PosEnd = (-PanelPatreons.rectTransform.sizeDelta.y);
            PosEndOriginal = (-PanelPatreons.rectTransform.sizeDelta.y);
        }

    }
}
