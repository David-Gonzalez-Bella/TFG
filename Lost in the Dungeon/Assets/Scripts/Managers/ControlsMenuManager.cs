using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsMenuManager : MonoBehaviour
{
    public ControlsBox box;
    public Button goToMainMenu;

    private void Start()
    {
        goToMainMenu.onClick.AddListener(Transitions.sharedInstance.TransitionToMainMenu);
        goToMainMenu.onClick.AddListener(AudioManager.sharedInstance.PlaySelectSound);
        if (!System.IO.File.Exists(Application.streamingAssetsPath + "/XML/Dialogues/dialogues.xml"))
        {
            SerializerXML.Serialize(Application.streamingAssetsPath + "/XML/Explanations/explanations.xml",
                                            new Dialogue
                                            {
                                                lines = new string[] { "Greetings traveller, and welcome to my dungeon. Let me teach you the basics to survive in this little hell of mine.",
                                                                                    "Use 'W', 'A', 'S' and 'D' keys to move around, and the LEFT CLICK in your mouse to attack with your sword. Try to remember these things, as they will be your tools to defeat all the CREATURES you will find in each room.",
                                                                                    "Don't be afraid though, little one, there are also some good guys around here. Those are the so-called ITEM SELLERS, bystanders that will offer you WEAPONS, IMPROVEMENTS and SPELLS in exchange of GOLD.",
                                                                                    "You can obtain it by breaking all the BOXES that lay in the rooms, but only after you have defeated all the creatures that will appear to protect it, so be ready to fight anytime.",
                                                                                    "If you finally get your hands on those shinny coins and buy a SPELL, you will be able to cast it using the SPACE key, but rememeber that, of course, it will cost you MANA. It will regenerate over time though, so don't worry too much about it.",
                                                                                    "Also, take into account that every time you speak with an ITEM SELLER, he will offer improvements for the items you chose in your first purchase, so choose wisely once you get to the first shop",
                                                                                    "Another important thing that you should know about is the EXPERIENCE system. With every enemy slain, you will obtain some experience, and in the panel you can display with the TAB key you can spend your level points to improve your skills.",
                                                                                    "Everytime you LEVEL UP your health will be significantly restored, and whenever you clear a room too. However, those creatures I mentioned earlier hit hard, so try to play cautious anyways.",
                                                                                    "Finally, your goal is simple: get to the exit of the dungeon. However, if your HEALTH drops down to zero, you will die, so don't play carelessly.",
                                                                                    "All that I have left to say is good luck, I wonder weather you will emerge victorious or perish in these catacombs..." },
                                                id = "DungeonMaster_S"
                                            });
        }
        box.dialogue = SerializerXML.Deserialize<Dialogue>(Application.streamingAssetsPath + "/XML/Explanations/explanations.xml");
        box.StartExplanation();
    }
}
