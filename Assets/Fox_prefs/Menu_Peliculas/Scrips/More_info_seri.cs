﻿
using System.Drawing;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonSharp.Video
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]

    public class More_info_seri : UdonSharpBehaviour
    {
  
        public string titulo;
        [TextArea(5, 20)]
        public string descripcion;
        public string year, genero;

        //public VRCUrl[] url_video = new VRCUrl[0];

        //public GameObject parnt_obj;
        public VRCUrl[] url_video;

        public bool Accion = false, Animacion = false, Anime = false, Aventura = false, CienciaFiccion = false, Comedia = false, Drama = false, Documental = false, Fantasia = false, Musical = false, Suspenso = false, Terror = false;

        public UnityEngine.UI.Image img_dest;
        public Text tit_dest, des_dest, year_dest, gen_dest;

        public Animator main_screen, info_screen;
        public Info_sys info_udon;
        public GameObject parnt_obj;
        public bool plis_no_mover=false;

        public void Start()
        {
            if (titulo.Length < 13 && plis_no_mover == false)
            {
                gameObject.transform.GetChild(0).GetComponent<ScrollRect>().enabled = false;
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = titulo;
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "  " + titulo + "  ";
            }
    }


        public void _do_stuff()
        {
            tit_dest.text = titulo;
            img_dest.sprite = gameObject.GetComponent<UnityEngine.UI.Image>().sprite;
            des_dest.text = descripcion;

            year_dest.text = "Año: " + year;
            gen_dest.text = "Genero: " + genero;

            //parnt_obj.transform.childCount;
            foreach (Transform child in parnt_obj.transform)
            {
                child.gameObject.SetActive(false);
            }

            for (int i = 0; i < url_video.Length; i++)
            {
                parnt_obj.transform.GetChild(i).gameObject.SetActive(true);
                parnt_obj.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = (i+1).ToString();
                parnt_obj.transform.GetChild(i).GetComponent<Url_holder_seri>().link_hold[0] = url_video[i];
            }

            //info_udon.url_buton = url_video;


            main_screen.Play("Get_off");
            info_screen.Play("Get_on");
        }
    }
}