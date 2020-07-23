using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakingItem : MonoBehaviour
{
    private Transform trans;
    public Rigidbody child_body;

    public GameObject child;

    public bool haveChild;

    private void Awake()
    {
        trans = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object Position") && !haveChild)
        {
            other.transform.GetChild(0).transform.SetParent(trans);           
            child = gameObject.transform.GetChild(0).gameObject;
            child.transform.position = trans.position;
            child_body = child.GetComponent<Rigidbody>();
            child_body.isKinematic = true;
            haveChild = true;           
        }
    }
}
