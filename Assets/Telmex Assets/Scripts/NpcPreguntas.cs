
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

public class NpcPreguntas : UdonSharpBehaviour
{
    [SerializeField]
    TextMeshProUGUI uiText;

    [SerializeField]
    Animator GloboTexto;

    [SerializeField]
    private UdonBehaviour[] ControladorRespuestas;

    [SerializeField]
    private string[] textToWrite;

    [SerializeField]
    float timeBtnChars=0.2f;

    [SerializeField]
    float timer=1f;

    [SerializeField]
    private int TipoDeRespuesta=0;

    [SerializeField]
    private AudioSource Sonido;

    private string textToWriteESC;
    private bool Escribir = false;
    private bool ResetGlobo = false;
    private int characterIndex;

  
    public void _WriteText()
    {
        ResetGlobo = false;
        for (int i = 0; i < ControladorRespuestas.Length; i++)
        {
            ControladorRespuestas[i].SendCustomEvent("ResetGlobotTXT");
        }
        GloboTexto.SetBool("AlphaCanvas", true);
        switch (TipoDeRespuesta)
        {
            case 0://SOLO ESCRIBIR
                if (textToWrite.Length != 1)
                {
                    textToWriteESC = textToWrite[Random.Range(0, textToWrite.Length)];
                }
                else
                {
                    textToWriteESC = textToWrite[0];
                }
                break;
            case 1://MOSTAR TIEMPO
                textToWriteESC = "Hoy es <b>" + System.TimeZoneInfo.ConvertTime(System.DateTime.Now, System.TimeZoneInfo.Local).ToString("dddd dd MMMM") + "</b> y son las <b>" + System.DateTime.Now.ToString("hh:mm tt");
                break;
            case 2://INFO DE LA SALA
                textToWriteESC = "El Master de la sala es <b>" + Networking.GetOwner(this.gameObject).displayName;
                break;
        }
        characterIndex = 0;
        Escribir = true;
        Sonido.Play();
    }
   

    public void ResetGlobotTXT()
    {
        ResetGlobo = true;
        uiText.text = "";
    }

    private void Update()
    {
        if (Escribir != false)
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                timer += timeBtnChars;
                characterIndex++;
                uiText.text = textToWriteESC.Substring(0, characterIndex);
                if(characterIndex>= textToWriteESC.Length|| ResetGlobo==true)
                {
                    Sonido.Stop();
                    Escribir = false;
                    return;
                }
            }
        }
    }
}
