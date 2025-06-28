using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject nameBox;
    [SerializeField] private Image next;
    [SerializeField] private int visibleLines = 3;
    [SerializeField] private int maxCharsPerLine = 55;
    [SerializeField] private float baseCharsPerSecond = 20;

    public bool IsOver { get; private set; }

    private Coroutine writingCoroutine;
    private string[] allLines;
    private int startLine = 0;
    private float charsPerSecond;

    private void Start() {
        next.gameObject.SetActive(false);
        IsOver = false;
        charsPerSecond = baseCharsPerSecond;
    }


    /// <summary>To set the complete multiline message</summary>
    /// <param name="speakerName">Name of the speaker</param>
    /// <param name="message">The actual multiline message dialog</param>
    public void SetMessage(string speakerName, string message) {
        if (speakerName == null || speakerName == "") nameBox.SetActive(false);
        else nameText.text = speakerName;

        // To manage lines so that they do't overflow
        List<string> wrappedLines = new List<string>();
        string[] lines = message.Split('\n');

        foreach (string line in lines) {
            string current = line;
            while (current.Length > maxCharsPerLine) {
                wrappedLines.Add(current.Substring(0, maxCharsPerLine));
                current = current.Substring(maxCharsPerLine);
            }
            wrappedLines.Add(current);
        }

        allLines = wrappedLines.ToArray();
        UpdateMessage();
    }


    /// <summary>To scroll the text in case they don't fit inside the box</summary>
    public void ScrollDown() {
        if (writingCoroutine != null) {
            charsPerSecond = baseCharsPerSecond * 3;
            return;
        }

        if (startLine + visibleLines < allLines.Length) {
            startLine++;
            UpdateMessage();
        }
    }


    /// <summary>To update the visible message inside the box (only the visible part)</summary>
    private void UpdateMessage() {
        // To ensure only one coroutine is running
        if (writingCoroutine != null) StopCoroutine(writingCoroutine);

        next.gameObject.SetActive(false);
        int endLine = Mathf.Min(startLine + visibleLines, allLines.Length);
        charsPerSecond = Mathf.Max(charsPerSecond, baseCharsPerSecond);
        messageText.text = "";

        if (startLine != 0) {
            messageText.text = string.Join('\n', allLines, startLine, endLine - startLine - 1) + '\n';
            writingCoroutine = StartCoroutine(WriteMessage(endLine - 1, endLine));
        }
        else {
            writingCoroutine = StartCoroutine(WriteMessage(startLine, endLine));
        }
    }


    /// <summary>To show typing effect using coroutine</summary>
    /// <param name="start">The starting line (inclusive)</param>
    /// <param name="end">The ending line (exxclusive)</param>
    private IEnumerator WriteMessage(int start, int end) {
        for (int i = start; i < end; i++) {
            foreach (char c in allLines[i]) {
                messageText.text += c;
                yield return new WaitForSeconds(1 / charsPerSecond);
            }
            messageText.text += '\n';
        }
        charsPerSecond = baseCharsPerSecond;
        if (end < allLines.Length) next.gameObject.SetActive(true);
        else IsOver = true;
        writingCoroutine = null;
    }
}
