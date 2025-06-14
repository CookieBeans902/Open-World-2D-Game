using UnityEditor.Rendering;
using UnityEngine;

namespace Game.Utils {
    static class VectorHandler {
        public static Vector3 AngleFromVector(Vector3 v) {
            float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
            return new Vector3(0, 0, angle);
        }

        public static Vector3 GenerateRandomDir(Vector3 baseDir, float del) {
            float a = Random.Range(-del, del);
            Quaternion rot = Quaternion.Euler(0, 0, a);
            Vector3 randomVector = rot * baseDir;

            return randomVector;
        }

        public static float GetDistance(Vector3 pointOne, Vector3 pointTwo) {
            Vector2 pointonePos = pointOne;
            Vector2 pointtwoPos = pointTwo;
            return (pointtwoPos - pointonePos).magnitude;
        }
    }
}