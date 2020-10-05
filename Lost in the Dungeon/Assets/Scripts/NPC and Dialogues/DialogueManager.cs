using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager sharedInstance;

    public Dictionary<string, Dialogue> dialogues = new Dictionary<string, Dialogue>();
    public Dialogue dungeonInfo;
    public Dialogue firstMissionStart;
    public Dialogue firstMissionEnd;
    public Dialogue findPetMissionStart;
    public Dialogue findPetMissionEnd;
    public Dialogue petDialogue;
    public Dialogue collectWeedsStart;
    public Dialogue collectWeedsEnd;

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
            //Inicialize dialogues
            dungeonInfo = new Dialogue { lines = new string[] { "dungeonInfo1", "dungeonInfo2", "dungeonInfo3", "dungeonInfo4" , "dungeonInfo5" , "dungeonInfo6" , "dungeonInfo7" , "dungeonInfo8" }, id = "EntranceGuard_S" };
            firstMissionStart = new Dialogue { lines = new string[] { "hi!", "how are you?", "bye!" }, id = "LostWarrior_S" };
            firstMissionEnd = new Dialogue { lines = new string[] { "thankyou1", "thankyou3", "thankyou3" }, id = "LostWarrior_E" };
            findPetMissionStart = new Dialogue { lines = new string[] { "hi!", "how are you?", "bye!" }, id = "GreatArchmage_S" };
            findPetMissionEnd = new Dialogue { lines = new string[] { "thankyou1", "thankyou3", "thankyou3" }, id = "GreatArchmage_E" };
            petDialogue = new Dialogue { lines = new string[] { "miau" }, id = "MrChopy_S" };
            collectWeedsStart = new Dialogue { lines = new string[] { "hi!", "how are you?", "bye!" }, id = "WiseMage_S" };
            collectWeedsEnd = new Dialogue { lines = new string[] { "thankyou1", "thankyou3", "thankyou3" }, id = "WiseMage_E" };

            //Serialize them, which means, create a xml document with its information if it does not exist already
            Dialogue[] xmlFile = new Dialogue[] { dungeonInfo, firstMissionStart, firstMissionEnd, findPetMissionStart, findPetMissionEnd, petDialogue, collectWeedsStart, collectWeedsEnd };
            SerializerXML.Serialize(Application.streamingAssetsPath + "/XML/Dialogues/dialogues.xml", xmlFile);
        }    
    }
}
