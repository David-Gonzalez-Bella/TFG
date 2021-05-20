using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager sharedInstance;

    public Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
    public Dialogue itemSeller_Dialogue;

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        Initialize();

        //Now that the file exists (so we can modify it easily from there, thats the point of all this) we can deserialize it and add the Dialogue objects to the dictionary
        Dialogue[] deserializedObjs = SerializerXML.Deserialize<Dialogue[]>(Application.streamingAssetsPath + "/XML/Dialogues/dialogues.xml");
        foreach (Dialogue d in deserializedObjs)
        {
            dialogues[d.id] = d;
        }
    }

    private void Initialize()
    {
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/XML/Dialogues/dialogues.xml"))
        {
            //Inicialize dialogues (with ordinary content, dialogues can be overwritten in the "dialogues.xml" file once it is created and, therefore, shown in the inspector)
            itemSeller_Dialogue = new Dialogue { lines = new string[] { "itemsInfo1", "itemsInfo2", "itemsInfo3", "byeMessageBought", "byeMessageLeave" }, id = "ItemSeller_S" };
            itemSeller_Dialogue = new Dialogue { lines = new string[] { "deny1" }, id = "ItemSeller_N" };

            //Serialize them, which means, create a xml document with its information if it does not exist already
            Dialogue[] xmlFile = new Dialogue[] { itemSeller_Dialogue };
            SerializerXML.Serialize(Application.streamingAssetsPath + "/XML/Dialogues/dialogues.xml", xmlFile);
        }    
    }
}
