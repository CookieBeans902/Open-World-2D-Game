using UnityEngine;


// Dialog class to easily serialize the dialogs in the editor
[System.Serializable]
public class Dialog {
    public string id;
    public string speaker;
    [TextArea(6, 6)]
    public string text;
}