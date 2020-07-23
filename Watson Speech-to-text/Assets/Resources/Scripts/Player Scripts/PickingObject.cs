using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingObject : MonoBehaviour
{
    public static bool isPicked;
    public static float takeTime;

    private void Start()
    {
        takeTime = 0;
    }

    void Update()
    {
        PickObject();
    }

    void PickObject()
    {
        RaycastHit hit;
        if (!isPicked)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isPicked = true;
                    hit.transform.gameObject.SetActive(false);
                    takeTime = Timer.timer;
                }
            }
        }

        if (takeTime < 0) takeTime = 0;
    }
}
