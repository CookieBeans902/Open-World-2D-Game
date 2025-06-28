using UnityEngine;

public class Healthbar : MonoBehaviour {
    [SerializeField] private Transform fill;

    public void SetFill(float value) {
        if (value > 1)
            fill.localScale = new Vector3(1, 1, 1);
        else if (value < 0)
            fill.localScale = new Vector3(0, 1, 1);
        else
            fill.localScale = new Vector3(value, 1, 1);
    }
}
