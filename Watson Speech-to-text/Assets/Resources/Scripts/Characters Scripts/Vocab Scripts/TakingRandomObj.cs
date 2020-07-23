using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TakingRandomObj : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private TakingItem takingItem;

    [SerializeField]
    private Transform Object_Parent;

    [SerializeField]
    private Transform Taking_Item_Position;

    private Transform[] Object_Child;
    private BoxCollider[] Object_Child_Collider;

    private string PUTTING_DOWN_ANIMATION = "puttingDown";
    private string SIDE_STEP_ANIMATION = "nextStep";
    private string TAKE_ITEM_ANIMATION = "takeItem";

    private SphereCollider handCollider;

    [HideInInspector]
    public int pickedObj;

    void Awake()
    {
        anim = GetComponent<Animator>();
        handCollider = takingItem.GetComponent<SphereCollider>();
        handCollider.enabled = false;
    }

    void Start()
    {
        Object_Child = new Transform[Object_Parent.childCount - 1]; //Need -1 bc the first element is the position for instanitiate
        Object_Child_Collider = new BoxCollider[Object_Parent.childCount - 1];

        for (int i = 0; i < Object_Child.Length; i++)
        {
            Object_Child[i] = Object_Parent.GetChild(i + 1);
            Object_Child_Collider[i] = Object_Child[i].GetComponent<BoxCollider>();
            //Debug.Log(Object_Child[i].gameObject.name);
        }
    }

    int RandomingObject()
    {
        int randNum = Random.Range(1, Object_Parent.childCount);
        return randNum;
    }

    void RetakingItem()
    {
        //Debug.Log("retaking....");
        anim.SetBool(TAKE_ITEM_ANIMATION, true);
        anim.SetBool(SIDE_STEP_ANIMATION, false);
    }

    void HandColliderHandler()
    {
        handCollider.enabled = true;
        Debug.Log("123");
    }

    void TakingItems()
    {
        if (takingItem.child)
        {
            takingItem.child_body.useGravity = true;
            takingItem.child_body.isKinematic = false;
            takingItem.child.transform.SetParent(Object_Parent.GetChild(pickedObj));
            takingItem.haveChild = false;
            takingItem.child = null;
            takingItem.child_body = null;
        }
        handCollider.enabled = false;
        BoxColliderHandler(Object_Child_Collider, 0, true);
    }

    public void PuttingDown()
    {
        anim.SetBool(PUTTING_DOWN_ANIMATION, true);
        anim.SetBool(SIDE_STEP_ANIMATION, false);
        anim.SetBool(TAKE_ITEM_ANIMATION, false);
    }

    void StartPickingObject() //Start a new turn
    {
        anim.SetBool(PUTTING_DOWN_ANIMATION, false);
        anim.SetBool(SIDE_STEP_ANIMATION, true);
    }

    void BossMovement()
    {
        //float speed = 0.5f;
        pickedObj = RandomingObject();
        //Debug.Log("coming to object number " + pickedObj);

        BoxColliderHandler(Object_Child_Collider, pickedObj, false);

        /*Vector3 pos = Object_Parent.GetChild(pickedObj).position; //??????
        pos.y = 0;
        transform.LookAt(pos);*/

        //Debug.Log("looking at " + Object_Parent.GetChild(pickedObj).name);

        float distance = Vector3.Distance(transform.position, Taking_Item_Position.GetChild(pickedObj).position);
        //Debug.Log("position: " + Taking_Item_Position.GetChild(pickedObj).name);
        //Debug.Log("distance: " + distance);

        if (distance > 0.5f)
        {
            //transform.position = Vector3.Lerp(transform.position, Taking_Item_Position.GetChild(pickedObj).position, speed / 2);
            //Debug.Log("distance > 0.5");
            Approaching(Taking_Item_Position.GetChild(pickedObj));
        }

        //transform.LookAt(pos);
        transform.LookAt(Camera.main.transform);
    }
    void Approaching(Transform target)
    {
        float speed = 0.5f;
        transform.position = Vector3.Lerp(transform.position, target.position, speed*4);
    }

    void BoxColliderHandler(BoxCollider[] boxColliders, int num, bool reset)
    {
        if (!reset)
        {
            for (int i = 0; i < boxColliders.Length; i++)
            {
                if (i != num - 1)
                {
                    boxColliders[i].enabled = false;
                }
                Debug.Log("456");
            }
        }
        else
        {
            for (int i = 0; i < boxColliders.Length; i++)
            {
                boxColliders[i].enabled = true;
            }
            Debug.Log("789");
        }
    }
}
