using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager ins;

    void Awake()
    {
        ins = this;
    }

    //List of items
    public ItemDatabase itemDatabase;

    //save function
    public void SaveItems()
    {
        //open a new xml file
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/suggestion_speech.xml", FileMode.Create);
        serializer.Serialize(stream, itemDatabase);
        stream.Close();
    }

    //load function
    public void LoadItems()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ItemDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/item_data.xml", FileMode.Open);
        itemDatabase = serializer.Deserialize(stream) as ItemDatabase;
        stream.Close();
    }
}

    [System.Serializable]
    public class ItemEntry
    {
        public string itemName;
        public int idx;
    }

    [System.Serializable]
    public class ItemDatabase
    {
        [XmlArray("ObjectList")]
        public List<ItemEntry> list = new List<ItemEntry>();
    }


