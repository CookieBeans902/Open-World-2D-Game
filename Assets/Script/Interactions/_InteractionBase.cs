using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBase : MonoBehaviour {
    public Transform MessagePoint;
    public string CharacterName;
    [SerializeField] protected MessageType messageType;
    [SerializeField] protected MessageBoxSO messageBoxes;
    [SerializeField] protected List<Dialog> dialogs;


    // T0 get the message pref baased on the message type
    protected GameObject messagePref {
        get {
            switch (messageType) {
                case MessageType.Bottom:
                    return messageBoxes.bottom;
                case MessageType.Middle:
                    return messageBoxes.middle;
                case MessageType.Top:
                    return messageBoxes.top;
                default:
                    return messageBoxes.floating;
            }
        }
    }


    /// <summary>To decide what happens when player presses the interact button</summary>
    public virtual void Interact() { }
}
