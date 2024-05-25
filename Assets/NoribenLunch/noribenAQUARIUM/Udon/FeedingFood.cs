using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FeedingFood : UdonSharpBehaviour
{

    public GameObject Package;
    public GameObject Food;
    public GameObject ForceField;
    public GameObject Tank;
    public GameObject FoodParticle;

    public Vector3 packagePos;
    public Vector3 foodPos;
    public Vector3 tankPos;
    public Rigidbody rbFood;
    public float destroyPos = -10f;
    public float waterHeight = .43f;
    public float foodDrag = 50;

    public bool droped;

    //経過時間用
    [SerializeField] private float countup = 0.0f;
    public float waitTime = 30.0f;

    void Start()
    {
        
        droped = true; //下まで落ちたよフラグ
        rbFood = Food.GetComponent<Rigidbody>(); 
        rbFood.useGravity = false;
        rbFood.isKinematic = false;
        ForceField.SetActive(false);
        FoodParticle.SetActive(false);

        tankPos = Tank.transform.position;
    }

    void Update() {
        
        //万が一、エサが消えなかったときのための処理
        //エサを落としてから一定時間、経過したら強制的に消す
        if (droped == false)
        {
            countup += Time.deltaTime;

            if (countup >= waitTime)
            {
                countup = 0;
                droped = true;
            }
        }
        else if (droped == true)
        {
            countup = 0;
        }

        dropFood();
    }


    public void dropFood()
    {
        packagePos = Package.transform.position; 
        foodPos = rbFood.transform.position;

        //下まで落ち済みのときの処理
        if(droped == true)
        {
            rbFood.useGravity = false;
            //rbFood.isKinematic = true;
            ForceField.SetActive(false);
            FoodParticle.SetActive(false);
            rbFood.MovePosition(new Vector3(packagePos.x,packagePos.y,packagePos.z));
        }

        //餌を落とすよ
        //if(goDrop == true && droped == false)
        if(droped == false)
        {
            ForceField.SetActive(true);
            FoodParticle.SetActive(true);
            rbFood.useGravity = true;
            //rbFood.isKinematic = false;

            //水面に落ちるまでは低Dragで、落ちたらDragを増やす
            rbFood.drag = 20;

            if (foodPos.y < tankPos.y + waterHeight + 0.05f)
            {
                rbFood.drag = foodDrag;
            }

            /*
            if (foodPos.y < tankPos.y + waterHeight - .03f)
            {
                rbFood.drag = 70;
            }
            */

            
        }
        
        //エサが消える位置(水槽の底から0.05m上)
        var destroyPos = tankPos.y + .05f;

        //エサが下まで行ったらdropedフラグを立てる
        if(foodPos.y < destroyPos)
        {
            //rbFood.useGravity = false;
            //rbFood.isKinematic = true;
            droped = true;
        }

    }

    public override void OnPickupUseDown()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SyncValue");
        //SyncValue(); //トリガーテスト用
        
        
    }

    public void SyncValue()
    {
        droped = false;
    }


}
