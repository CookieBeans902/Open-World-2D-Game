using UnityEngine;

public enum EffectType {
    RecoverHp,
    AddExp,
    BuffAtkSpeed,
    BuffSpeed,
}

public class ItemEffectManager : MonoBehaviour {
    public static ItemEffectManager Instance;
    private void Awake() {
        if (CharacterManager.Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (CharacterManager.Instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    public void ExecuteEffect(EffectType effectType, int value) {
        switch (effectType) {
            case EffectType.RecoverHp:
                RecoverHp(value);
                break;
            case EffectType.AddExp:
                AddExp(value);
                break;
            case EffectType.BuffAtkSpeed:
                BuffAtkSpeed(value);
                break;
            case EffectType.BuffSpeed:
                BuffSpeed(value);
                break;
        }
    }

    private void RecoverHp(int amount) {
        PlayerStats stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (stats == null) return;

        int a = (int)Random.Range(amount * 0.7f, amount * 1.3f);
        stats.RecoverHp(amount);
    }

    private void AddExp(int amount) {
        Character charData = CharacterManager.Instance.characters[0];
        if (charData == null) return;

        charData.AddExp(amount);
    }

    private void BuffAtkSpeed(int amount) {
        PlayerShared shared = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShared>();
        if (shared == null) return;

        shared.SetAtkSpeedBuff(amount);
    }

    private void BuffSpeed(int amount) {
        PlayerMove move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        if (move == null) return;

        move.SetSpeedBuff(amount);
    }
}
