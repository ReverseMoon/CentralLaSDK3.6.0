using System;
using UdonSharp;
using UnityEngine.UI;
using VRC.SDKBase;
using UnityEngine;

public class PlayerList : UdonSharpBehaviour
{
    public GameObject SyncPool;
    public Text[] TextArray;
    public GameObject[] Iconos;

    public int UpdatesPerFrame = 5;

    [UdonSynced]
    private string master_name;

    public string local_name;

    private PlayerListSync[] pool;
    private int curUpdate = -1;

    private string[] texts;


    void Start()
    {
        this.local_name = Networking.LocalPlayer.displayName;

        pool = new PlayerListSync[SyncPool.transform.childCount];
        var i = 0;
        foreach (Transform t in SyncPool.transform)
        {
            pool[i] = t.gameObject.GetComponent<PlayerListSync>();
            i++;
        }

        texts = new string[pool.Length + 1];
    }

    private string Format(string name, bool master)
    {
        var suffix = "";
        if (master)
        {
            suffix += " <color=#ffee00>[master]</color>";
        }

        return $"{name}{suffix}";
    }

    void Update()
    {
        var changed = false;
        for (int i = 0; i < UpdatesPerFrame; i++)
        {
            if (curUpdate == -1)
            {
                var tmp = Format(master_name, true);
                if (texts[0] != tmp)
                {
                    texts[0] = tmp;
                    changed = true;
                }
            }
            else
            {
                string tmp = null;
                var cur = pool[curUpdate];
                var owner = Networking.GetOwner(cur.gameObject);
                if (!owner.isMaster)
                {
                    if (cur.Name != owner.displayName)
                    {
                        tmp = $"{owner.displayName}";
                    }
                    else
                    {
                        tmp = Format(cur.Name, false);
                    }

                    if (owner.isLocal)
                    {
                        cur.ManualUpdate();
                    }
                }

                if (texts[curUpdate + 1] != tmp)
                {
                    texts[curUpdate + 1] = tmp;
                    changed = true;
                }
            }

            curUpdate++;
            if (curUpdate >= pool.Length)
            {
                curUpdate = -1;
            }
        }

        if (changed)
        {
            for (int i = 0; i < TextArray.Length; i++)
            {
                this.TextArray[i].text = "";
            }
            for (int M = 0; M < Iconos.Length; M++)
            {
                Iconos[M].SetActive(false);
            }


            string[] textsTemp=new string[64];
            int band = 0;
            for (int k = 0; k < texts.Length; k++)
            {
                if (texts[k] != null)
                {
                    textsTemp[band] = texts[k];
                    band++;
                }

            }
    
            for (int j = 0; j < textsTemp.Length; j++)
            {
                if (textsTemp[j] != null) { 
                    TextArray[j].text = textsTemp[j];
                    Iconos[j].SetActive(true);
                }
            }
        }

        if (Networking.IsMaster && Networking.IsOwner(this.gameObject))
        {
            if (local_name != master_name)
            {
                master_name = local_name;
                this.RequestSerialization();
            }
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        var count = VRCPlayerApi.GetPlayerCount();
        if (Networking.IsMaster && Networking.IsOwner(this.gameObject) && !player.isMaster&&VRCPlayerApi.GetPlayerCount()<=32)
        {
            foreach (var sync in pool)
            {
                if (Networking.IsOwner(sync.gameObject))
                {
                    Debug.Log($"[PlayerList] giving out object #{sync.gameObject.name} from pool to: {player.displayName} #{player.playerId}");
                    Networking.SetOwner(player, sync.gameObject);
                    break;
                }
            }
        }
    }
}
