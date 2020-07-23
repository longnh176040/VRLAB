using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstanitiateObjPos : MonoBehaviour
{
    public Transform Object_Pos;
    public Transform Obj_Parent;
    public Transform Ins_Pos;

    public Transform Taking_Item_Pos;
    public Transform Taking_Item_Par;
    public Transform Ins_TIP;

    public Transform Learning_Obj;

    public XMLParsing parsingXML;

    public List<string> picked_Object; //use to store name of objects that are rendered to the scene
    private List<int> ordered_Object;   //use to store id of object that is rendered to the scene
    

    void Awake()
    {
        RenderingPosition();
        ordered_Object = new List<int>();
        picked_Object = new List<string>();
    }

    void Start()
    {
        //RenderingObject(picked_Object);
    }

    public static int RandomingObject(List<int> pickedObj)             //use to randoming id of objs for rendering
    {
        int idx = Random.Range(0, 37);     //need fixed later for more objs randoming

        if (pickedObj.Count == 0) 
        {
            pickedObj.Add(idx);
            return idx;
        }
        else
        {
            for (int i = 0; i < pickedObj.Count; i++)
            {
                if (idx == pickedObj[i]) return RandomingObject(pickedObj);
                else continue;
            }
            pickedObj.Add(idx);
            return idx;
        }        
    }

    public void RenderingObject(List<string> pickedObj)    //Use for rendering random Obj
    {
        for (int i = 0; i < Obj_Parent.childCount-1; i++)                
        {
            string obj_name = parsingXML.dic[RandomingObject(ordered_Object).ToString()]; //rendering random obj
            pickedObj.Add(obj_name);
            GameObject obj_render = Instantiate(Resources.Load("Prefabs/Learning Object/" + obj_name), new Vector3(Ins_Pos.position.x + i * 0.65f, Ins_Pos.position.y - 0.2f, Ins_Pos.position.z + 0.1f), Quaternion.identity, Learning_Obj) as GameObject;
            obj_render.name = obj_name;
        }
    }

    public void RenderingPickedList(List<string> pickedObj) //Use for rendering picked Obj
    {
        for (int i = 0; i < Obj_Parent.childCount - 1; i++)
        {
            string key = parsingXML.dic.FirstOrDefault(x => x.Value == pickedObj[i]).Key; //Find key of object in the dictionary
            ordered_Object.Add(int.Parse(key));
            GameObject obj_render = Instantiate(Resources.Load("Prefabs/Learning Object/" + pickedObj[i]), new Vector3(i * 0.65f + Ins_Pos.position.x, Ins_Pos.position.y - 0.2f, Ins_Pos.position.z + 0.1f), Quaternion.identity, Learning_Obj) as GameObject;
            obj_render.name = pickedObj[i];
        }
    }

    public void ResetObject()
    {
        for (int i = 1; i < Obj_Parent.childCount; i++)
            Destroy(Obj_Parent.GetChild(i).GetChild(0).gameObject);
    }

    private void RenderingPosition () //instanitiate object pos and taking obj pos
    {
        for (int i = 0; i < 6; i++)          
        {
            Transform Obj_Pos;
            Obj_Pos = Instantiate(Object_Pos, new Vector3(Ins_Pos.position.x + i * 0.65f , Ins_Pos.position.y, Ins_Pos.position.z + 0.05f), Quaternion.identity, Obj_Parent) as Transform;
            Obj_Pos.gameObject.tag = "Object Position";
            Obj_Pos.gameObject.name = "Object " + (i + 1).ToString();

            /*if (i<5)
            {*/
                Transform TI_Pos;
                TI_Pos = Instantiate(Taking_Item_Pos, new Vector3(Ins_TIP.position.x + i * 0.8f, Ins_TIP.position.y, Ins_TIP.position.z), Quaternion.identity, Taking_Item_Par) as Transform;
                TI_Pos.gameObject.tag = "Taking Item Position";
                TI_Pos.gameObject.name = "Position " + (i + 1).ToString();
            //}
        }
    }

}
