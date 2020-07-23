using System.Collections;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public SpeechSuggestion player;
    private RectTransform trans;

    private Vector3 target;

    private void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    private void Update()
    {
        target = new Vector3(player.transform.position.x, trans.position.y, -player.transform.position.z);
        trans.localRotation = Quaternion.Slerp(trans.localRotation, Quaternion.LookRotation(target), 100f);
    }

    /*void LateUpdate()
    {
        if (player.changingScene == true) //should check when endOfConversation instead but got ERROR
        {
            Debug.Log("Changing scene ");
            StartCoroutine(DelayBeforeChanging());
            player.changingScene = false;
        }
        //StartCoroutine(DelayBeforeChanging());
    }

    void ChangingPositon()
    {
        transform.LookAt(target);
        switch (SpeechSuggestion.sceneCount)
        {
            case 1:
                trans.localPosition = new Vector3(8f, -67f, 10f);
                //trans.localRotation = Quaternion.Euler(0f, 150f, 0f);
                break;

            case 2:
                trans.localPosition = new Vector3(9.5f, -67f, -3f);
                //trans.localRotation = Quaternion.Euler(0f, 90f, 0f);
                break;

            case 3:
                trans.localPosition = new Vector3(-18.5f, -67f, 6.7f);
                //trans.localRotation = Quaternion.Euler(0f, 120f, 0f);
                break;

            case 4:
                trans.localPosition = new Vector3(-17.5f, -67f, 11.2f);
                //trans.localRotation = Quaternion.Euler(0f, 90, 0f);
                break;
            case 5:
                trans.localPosition = new Vector3(-10f, -67f, -1.5f);
                //trans.localRotation = Quaternion.Euler(0f, 200, 0f);
                break;
            case 6: //+7
                trans.localPosition = new Vector3(-18.5f, -67f, 13.5f);
                //trans.localRotation = Quaternion.Euler(0f, 290, 0f);
                break;
            case 7: 
                trans.localPosition = new Vector3(-18.5f, -67f, 13.5f);
                //trans.localRotation = Quaternion.Euler(0f, 290, 0f);
                break;
            case 8:
                trans.localPosition = new Vector3(-25f, -67f, 2.5f);
                //trans.localRotation = Quaternion.Euler(0f, 20, 0f);
                break;
            case 9: // +10, 11 == 4
                trans.localPosition = new Vector3(-17.5f, -67f, 11.2f);
                //trans.localRotation = Quaternion.Euler(0f, 90, 0f);
                break;
        }
    }

    IEnumerator DelayBeforeChanging()
    {
        yield return new WaitForSeconds(3f);
        ChangingPositon();
    }*/
}
