using System.Collections.Generic;

using UnityEngine;

public class EnemyDrop : MonoBehaviour {
    [SerializeField] List<Transform> collectibles;
    [SerializeField] int maxCount = 3;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            int count = Random.Range(0, maxCount);

            for (int i = 0; i < count + 1; i++) {
                int id = (int)Random.Range(0, collectibles.Count);
                Collectible c = Instantiate(collectibles[id]).GetComponent<Collectible>();
                c.transform.position = transform.position;

                Vector2 target = (Vector2)transform.position + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));

                c.Jump(6, 8, 0.6f);
            }
        }
    }
}
