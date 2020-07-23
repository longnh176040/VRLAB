using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssigningChild : MonoBehaviour
{
    public bool assignedChild;

    [SerializeField]
    private Transform objectPosition;
    [SerializeField]
    private Transform learningObject;

    private void Update()
    {
        if (!assignedChild)
        {
            AssignChild(objectPosition, learningObject);
            assignedChild = true;
        }
    }

    public void AssignChild(Transform objectPos, Transform learningObj)
    {
        for (int i = 1; i < objectPos.childCount; i++)
        {
            learningObj.GetChild(0).SetParent(objectPos.GetChild(i));
        }
    }

}
