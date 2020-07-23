using UnityEngine;
using UnityEngine.UI;

public class CustomLaserPointer : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 20f;
    OvrAvatar ovrAvatar;

    //OVRCameraRig cameraRig;
    //public Text text;

    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.SetWidth(laserWidth, laserWidth);
        //cameraRig = FindObjectOfType<OVRCameraRig>();
        ovrAvatar = FindObjectOfType<OvrAvatar>();
    }
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            ShootLaserFromTargetPosition(/*cameraRig.rightHandAnchor.localPosition*/
            ovrAvatar.HandRight.transform.position, ovrAvatar.HandRight.transform.forward, laserMaxLength);
            //text.text = "laser pointer triggered"; //debug text
            laserLineRenderer.enabled = true;
        }
        else
        {
            //text.text = "";
            laserLineRenderer.enabled = false;
        }
    }
    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        Vector3 endPosition = targetPosition + (length * direction);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, length))
        {
            endPosition = raycastHit.point;
            Debug.Log("CHAM:" + raycastHit.collider.name);
        }
        laserLineRenderer.SetPosition(0, targetPosition);
        laserLineRenderer.SetPosition(1, endPosition);
    }
} 

