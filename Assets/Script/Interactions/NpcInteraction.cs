using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class NpcInteraction : InteractionBase {
    private enum id {
        Welcome = 0,
        Test1 = 1,
        Test2 = 2,
        Test3 = 3,
    }
    private List<NpcInteraction> npcs;
    private MessageManager msg;
    private int i = 0;

    private void Start() {
        CheckForNpcs();
    }

    public override void Interact() {
        ShowMessage();
    }

    /// <summary>Custom logic to decide when which message is shown</summary>
    private void ShowMessage() {
        if (msg == null) {
            NpcInteraction npc = null;
            if (i == (int)id.Welcome) npc = this;
            if (i == (int)id.Test1) npc = GetNpc("Villager2");
            else if (i == (int)id.Test2) npc = GetNpc("Villager3");
            else if (i == (int)id.Test3) npc = GetNpc("Player");

            if (npc != null) {
                msg = Instantiate(messagePref).GetComponent<MessageManager>();
                if (messageType == MessageType.Floating) msg.transform.position = npc.MessagePoint.position;
                msg.SetMessage(npc.CharacterName, dialogs[i].text);
            }
            else {
                i = 0;
            }
        }
        else {
            if (!msg.IsOver) {
                msg.ScrollDown();
            }
            else {
                Destroy(msg.gameObject);
                msg = null;
                i++;
                ShowMessage();
            }
        }
    }


    /// <summary>To check for all npcs and player in a given range and store them in a list, 
    /// useful when floating message is to be displayed on the heads of different characters</summary>
    private void CheckForNpcs() {
        npcs = new List<NpcInteraction>();
        int layerMask = LayerMask.GetMask("NPC", "Player");
        float radius = 6;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, layerMask);
        foreach (Collider2D hit in hits) {
            NpcInteraction npc = hit.GetComponent<NpcInteraction>();
            if (npc != null) {
                npcs.Add(npc);
            }
        }
    }

    /// <summary>To get npc in the list, returns null if not present</summary>
    private NpcInteraction GetNpc(string name) {
        foreach (NpcInteraction npc in npcs) {
            if (npc.CharacterName == name) return npc;
        }
        return null;
    }
}
