using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;

public class XMLParsing : MonoBehaviour
{
    public List<Dictionary<string, string>> allTextDic;
    public Dictionary<string, string> dic;

    void Awake()
    {
        allTextDic = parseFile();
        dic = allTextDic[0];
    }

    public List<Dictionary<string, string>> parseFile()
    {
        TextAsset txtXmlAsset = Resources.Load<TextAsset>("StreamingFiles/XML/item_data");
        var doc = XDocument.Parse(txtXmlAsset.text);

        var allDict = doc.Element("ItemDatabase").Elements("ObjectList");
        List<Dictionary<string, string>> allTextDic = new List<Dictionary<string, string>>();
        foreach (var oneDict in allDict)
        {
            var twoStrings = oneDict.Elements("ItemEntry");
            var nameString = twoStrings.Elements("itemName");
            var idxString = twoStrings.Elements("idx");

            XElement[] elementName = new XElement[twoStrings.Count()];
            XElement[] elementIdx = new XElement[twoStrings.Count()];

            Dictionary<string, string> dic = new Dictionary<string, string>();

            for (int i = 0; i < twoStrings.Count(); i++)
            {
                elementName[i] = nameString.ElementAt(i);
                elementIdx[i] = idxString.ElementAt(i);

                string objName1 = elementName[i].ToString().Replace("<itemName>", "").Replace("</itemName>", "").Trim();
                string objIdx1 = elementIdx[i].ToString().Replace("<idx>", "").Replace("</idx>", "").Trim();

                dic.Add(objIdx1, objName1);
            }

            allTextDic.Add(dic);
        }

        return allTextDic;

    }
}
