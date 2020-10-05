using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    public static MissionsManager sharedInstance;

    public Dictionary<string, Mission> missions = new Dictionary<string, Mission>();
    public List<Mission> missionText = new List<Mission>();
    private int nMissions;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        nMissions = Missions_Texts.sharedInstance.missions.Length;

        Initialize();

        //Now that the file exists (so we can modify it easily from there, thats the point of all this) we can deserialize it and add the Dialogue objects to the dictionary
        List<Mission> deserializedObjs = SerializerXML.Deserialize<List<Mission>>(Application.streamingAssetsPath + "/XML//Missions/missions.xml");
        foreach (Mission m in deserializedObjs)
        {
            missions[m.id] = m;
        }
    }

    private void Initialize()
    { 
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/XML/Missions/missions.xml"))
        {
            //Inicialize mission texts
            for (int i = 0; i < nMissions; i++)
            {
                missionText.Add(new Mission { description = "Description", id = "Mission", type = "Type", quantity = 0, progress = 0, completed = false, exp = 0});
            }

            //Serialize them, which means, create a xml document with its information if it does not exist already
            SerializerXML.Serialize(Application.streamingAssetsPath + "/XML/Missions/missions.xml", missionText);
        }
    }

    public void ResetMissionsProgress()
    {
        foreach (var item in missions.Values)
        {
            item.progress = 0;
            item.completed = false;
        }
    }
}
