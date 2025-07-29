using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour {
    public static CharacterManager Instance { get; private set; }

    /* A scriptable Object which has a list of characters already defined for easy addition and removal of characters,
       you define it in the editor and assign the CharacterSO objects to it's feilds. (Check the Scripts/ScriptableObject folder) */
    [SerializeField] private CharacterListSO initCharacterList;

    /* A scriptable Object which has a list of equipments already defined for easy addition and removal of equipments,
       you define it in the editor and assign the EquipmentSO objects to it's feilds. (Check the Scripts/ScriptableObject folder) */
    [SerializeField] private EquipmentListSO initEquipmentList;

    // to mantain a list of characters in the team at current moment
    public List<Character> characters { get; private set; }

    private void Awake() {
        if (CharacterManager.Instance == null) {
            // this is to make this object persist across scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (CharacterManager.Instance != this) {
            // destroy self if an instance is already present to ensure there is only one manager
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        characters = new List<Character>();
        AddCharacter(initCharacterList.Character1);
        AddCharacter(initCharacterList.Character2);
        AddCharacter(initCharacterList.Character3);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            // string nextScene = "StatsTest1";
            characters[0].ShowStats();
        }
    }


    /// <summary>To add a character to the team</summary>
    /// <param name="cahracter">The character you want to add</param>
    private void AddCharacter(CharacterSO character) {
        Character c = new Character(character);

        characters.Add(c);
    }

    /// <summary>To remove a character from the team by name</summary>
    /// <param name="name">Name of the character you want to remove</param>
    public void RemoveCharacter(string name) {
        foreach (Character c in characters) {
            if (c.characterName == name) {
                characters.Remove(c);
                return;
            }
        }
    }

    /// <summary>To remove a character from the team by index</summary>
    /// <param name="index">Index of the character you want to remove</param>
    public void RemoveCharacter(int index) {
        if (index < 0 || index >= characters.Count)
            return;
        characters.RemoveAt(index);
    }
}
