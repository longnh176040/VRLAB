using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBehavior : MonoBehaviour
{
    private AudioSource audioSource;
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

    public bool resetTurn;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
        }
    }

    private void Update()
    {
        if (GameController.i == 6 && GameController.learning_Obj.Count < 6 && !resetTurn) 
            StartCoroutine(ResetTurn());
    }

    void TakingItems()
    {      
        if (takingItem.child)
        { 
            takingItem.child.gameObject.tag = "Untagged";
            takingItem.child_body.useGravity = true;
            takingItem.child_body.isKinematic = false;
            takingItem.child.transform.SetParent(Object_Parent.GetChild(GameController.i));
            takingItem.haveChild = false;
            takingItem.child = null;
            takingItem.child_body = null;
        }
        handCollider.enabled = false;
        BoxColliderHandler(Object_Child_Collider, 0, true);
        if (GameController.i < 6)
            SideStep();
    }

    public void PuttingDown()
    {
        anim.SetBool(PUTTING_DOWN_ANIMATION, true);
        anim.SetBool(SIDE_STEP_ANIMATION, false);
        anim.SetBool(TAKE_ITEM_ANIMATION, false);
    }

    void SideStep() //Start the Walking Animation
    {
        anim.SetBool(SIDE_STEP_ANIMATION, true);
        anim.SetBool(PUTTING_DOWN_ANIMATION, false);
    }

    void RetakingItem()
    {
        if (GameController.i != 6)
            BossMovement();

        anim.SetBool(TAKE_ITEM_ANIMATION, true);
        anim.SetBool(SIDE_STEP_ANIMATION, false);
    }

    void PlayingAudio(bool ok)
    {
        if (ok)
            if (takingItem.haveChild)
            {
                audioSource = takingItem.child.GetComponent<AudioSource>();
                audioSource.Play();
            }
    }

    void BossMovement()
    {
        float speed = 0.5f;

        if (GameController.i < 6 && GameController.i > 0)
        {
            Vector3 pos = Object_Parent.GetChild(GameController.i+1).position;
            pos.y = 0;
            transform.LookAt(pos);

            BoxColliderHandler(Object_Child_Collider, GameController.i , false);

            float distance = Vector3.Distance(transform.position, Taking_Item_Position.GetChild(GameController.i-1).position);
            //Debug.Log("position: " + Taking_Item_Position.GetChild(GameController.i - 1).name);
            //Debug.Log("distance: " + distance);

            if (distance > 0f)           
                transform.position = Vector3.Lerp(transform.position, Taking_Item_Position.GetChild(GameController.i-1).position, speed/2);           
        }
        else if (GameController.i == 0) BoxColliderHandler(Object_Child_Collider, GameController.i, false);
    }

    IEnumerator ResetTurn()
    {
        yield return new WaitForSeconds(3f);
        SideStep();

        Vector3 pos = Object_Parent.GetChild(1).position;
        pos.y = 0;
        transform.LookAt(pos);

        //BoxColliderHandler(Object_Child_Collider, GameController.i, false);

        Vector3 resetPos = new Vector3(Taking_Item_Position.GetChild(1).position.x - 1, Taking_Item_Position.GetChild(1).position.y, Taking_Item_Position.GetChild(1).position.z);

        float distance = Vector3.Distance(transform.position, resetPos);

        if (distance > 0.5f)
            transform.position = Vector3.Lerp(transform.position, resetPos, 0.5f / 4);
        else {
            RetakingItem();
            resetTurn = true; 
        }
    }

    void HandColliderHandler()
    {
        handCollider.enabled = true;
    }

    void BoxColliderHandler(BoxCollider[] boxColliders, int num, bool reset)
    {
        if (!reset)
        {
            for (int i = 0; i < boxColliders.Length; i++)
                if (i != num)
                    boxColliders[i].enabled = false;
        }
        else
        {
            for (int i = 0; i < boxColliders.Length; i++)
                boxColliders[i].enabled = true;
        }
    }

}
