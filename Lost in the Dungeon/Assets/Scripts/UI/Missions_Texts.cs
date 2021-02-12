using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Missions_Texts : MonoBehaviour
{
    public static Missions_Texts sharedInstance { get; private set; }

    public TMP_Text[] missions;
    public int missionIndex = 0;
    private PlayerController player;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void AddMission(Mission mission)
    {
        if (!player.activeMissions.Contains(mission) && missionIndex < missions.Length)
        {
            player.activeMissions.Add(mission);
            UpdateMissionText(missionIndex, mission);
            missionIndex++;
        }
    }

    public void ClearMission(Mission mission)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            if (missions[i].text.ToLower().Contains(mission.description.ToLower()))
            {
                missions[i].color = new Color(0.75f, 1.0f, 0.7f, 1.0f);
            }
        }
    }


    private void UpdateMissionText(int index, Mission mission)
    {
        missions[index].text = mission.description;
        if (mission.type.CompareTo("Recollection") == 0)
        {
            missions[index].text += ": " + mission.progress + "/" + mission.quantity.ToString();
        }
    }

    public void CheckUpdateMission(Mission m)
    {
        for (int i = 0; i < player.activeMissions.Count; i++)
        {
            if (player.activeMissions[i] == m && !player.activeMissions[i].completed)
            {
                //if (player.activeMissions[i].description.ToLower().Contains("kill " + player.activeMissions[i].quantity.ToString() + " enemies"))
                if (player.activeMissions[i].type.CompareTo("Recollection") == 0)
                {
                    player.activeMissions[i].progress++;
                    UpdateMissionText(i, player.activeMissions[i]);
                    if (player.activeMissions[i].progress == player.activeMissions[i].quantity)
                    {
                        player.activeMissions[i].completed = true;
                    }
                }
            }
        }
    }

    public void EmptyMissions()
    {
        for (int i = 0; i < missions.Length; i++)
        {
            missions[i].color = new Color(1.0f, 0.9f, 0.8f, 1.0f);
            missions[i].text = "";
        }
        //for (int i = 0; i < missions.Length; i++)
        //{
            //missions[i].text = "";
        //}
    }
}
