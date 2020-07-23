using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
ONLY USE FOR SCENE 1 
*/

public class InstanitiateLearningObjects : MonoBehaviour
{
    public Transform Learning_Obj;
    public Transform Ins_Pos;
    public XMLParsing parsingXML;

    private List<string> picked_Object; //use to store name of objects that are rendered to the scene
    private List<int> ordered_Object;   //use to store id of object that is rendered to the scene

    void Start()
    {
        ordered_Object = new List<int>();
        picked_Object = new List<string>();
        RenderingObject(/*picked_Object*/ GameController.learning_Obj);
    }

    private void RenderingObject(List<string> pickedObj)
    {
        for (int i = 0; i < 6; i++)             //Shouldn't set i = 6...... (_ _")
        {
            string obj_name = parsingXML.dic[InstanitiateObjPos.RandomingObject(ordered_Object).ToString()]; //rendering random obj
            pickedObj.Add(obj_name);
            GameObject obj_render = Instantiate(Resources.Load("Prefabs/Learning Object/" + obj_name), new Vector3(i * 0.65f + Ins_Pos.position.x, Ins_Pos.position.y + 0.1f, Ins_Pos.position.z + 0.1f), Quaternion.identity, Learning_Obj) as GameObject;
            obj_render.name = obj_name;

            /* string key = parsingXML.dic.FirstOrDefault(x => x.Value == GameController.learning_Obj[i]).Key; //Find key of object in the dictionary
             ordered_Object.Add(int.Parse(key));
             GameObject obj_render = Instantiate(Resources.Load("Prefabs/Learning Object/" + GameController.learning_Obj[i]), new Vector3(i * 0.65f + Ins_Pos.position.x, Ins_Pos.position.y + 0.1f, Ins_Pos.position.z + 0.1f), Quaternion.identity, Learning_Obj) as GameObject;
             */
        }
    }
}
