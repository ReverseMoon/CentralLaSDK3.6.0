
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class publ_sys : UdonSharpBehaviour
{
    public GameObject parnt_obj;
    public int secs_a_esperar=5;
    int page,lst_page,size;

    void Start()
    {
        size = parnt_obj.transform.childCount;
        if (size>1) {
            page = 0; lst_page = size - 1;

            for (int i = 0; i < size; i++)
            {
                parnt_obj.transform.GetChild(i).GetComponent<Animator>().Play("GetOut");
            }

            do_cicle(); 
        }
    }

    public void do_cicle()
    {
        if (page == size) { page = 0; lst_page = size - 1; }

        parnt_obj.transform.GetChild(page).GetComponent<Animator>().Play("GetIn");
        parnt_obj.transform.GetChild(lst_page).GetComponent<Animator>().Play("GetOut");

        page++;
        lst_page = page - 1;
        SendCustomEventDelayedSeconds("do_cicle", secs_a_esperar);
    }
}
