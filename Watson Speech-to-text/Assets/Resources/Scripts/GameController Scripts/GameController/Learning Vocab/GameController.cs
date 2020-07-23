using Oculus.Platform.Samples.VrBoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Boss Male")]
    [Tooltip("The one who asks you so many questions")]
    [SerializeField]
    private BossBehavior takeItem;

    [Header("Boss's hand")]
    [SerializeField]
    private TakingItem objectName;

    private InstanitiateObjPos instanitiateObj;
    private AssigningChild assigning;

    public Image[] picking_Obj;
    public Sprite Default;
    public Sprite Accept;
    public Sprite Deny;

    public Text objectName_text;
    public GameObject background;

    public static List<string> learning_Obj;

    private bool [] isChosen;
    private bool isClicked, turnOnDialogue;

    public static int i;        //use for counting turns

    private bool resetObj;

    private void Awake()
    {
        instanitiateObj = gameObject.GetComponent<InstanitiateObjPos>();
        assigning = gameObject.GetComponent<AssigningChild>();

        objectName_text.gameObject.SetActive(false);
        background.gameObject.SetActive(false);

        isChosen = new bool[6];
        learning_Obj = new List<string>();
        objectName_text.text = "";
    }

    void Start()
    {
        instanitiateObj.RenderingObject(instanitiateObj.picked_Object);
    }

    void Update()
    {
        RenderDialogue();
        CheckInput();

        if (isClicked)
        {
            takeItem.PuttingDown();
            isClicked = false;
            i++;
        }

        if (learning_Obj.Count == 6 && !resetObj)
        {
            StartCoroutine(LoadScene(2));
            resetObj = true;
        }
        //Reload for more options
        //learning_Obj must be equal to 6 
        else if (i == 6 && learning_Obj.Count < 6 && !resetObj&& !resetObj)
        {
            StartCoroutine(ResetObject());
            resetObj = true;
        }
        else if (i != 6) 
        { 
            resetObj = false;
            takeItem.resetTurn = false;
        }
    }
    private void CheckInput()
    {
        if (Input.GetKeyUp(KeyCode.A) /*OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch)*/)
        {
            //Debug.Log("123");
            isChosen[i] = true;
            isClicked = true;
            RenderOption(picking_Obj, i);
            learning_Obj.Add(objectName.child.name);
        }
        else if (Input.GetKeyUp(KeyCode.D) /*OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch)*/)
        {
            //Debug.Log("456");
            isChosen[i] = false;
            isClicked = true;
            RenderOption(picking_Obj, i);
        }
    }

    IEnumerator ResetObject()
    {
        yield return new WaitForSeconds(2.5f);
        instanitiateObj.ResetObject();
        yield return new WaitForSeconds(1.1f);
        instanitiateObj.RenderingObject(instanitiateObj.picked_Object);

        for (int j = 0; j < picking_Obj.Length; j++)
            picking_Obj[j].gameObject.GetComponent<Image>().sprite = Default;

        assigning.assignedChild = false;
        i = 0;
    }

    public static IEnumerator LoadScene(int level)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(level);
    }

    private void RenderOption(Image[] images, int i)
    {
       if (isChosen[i])
            images[i].gameObject.GetComponent<Image>().sprite = Accept;
       else
            images[i].gameObject.GetComponent<Image>().sprite = Deny;
    }

    public static void CheckDuplicateElement(List<string> checkList, string name)
    {
        if (!checkList.Contains(name))
            checkList.Add(name);
    }

    private void RenderDialogue() //Call in Update()
    {
        if (objectName.haveChild && !turnOnDialogue)
        {
            objectName_text.gameObject.SetActive(true);
            background.gameObject.SetActive(true);
            objectName_text.text = objectName.child.name;
            turnOnDialogue = true;
        }
        else if (!objectName.haveChild && turnOnDialogue)
        {
            objectName_text.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            objectName_text.text = "";
            turnOnDialogue = false;
        }
    }
}
