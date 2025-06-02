using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils {
    public class FunctionTimer {
        private static List<FunctionTimer> timerList;
        private static GameObject initGameObject;

        private Action action;
        private float timer;
        private bool isDestroyed;
        private GameObject gameObject;
        private string timerName;

        FunctionTimer(Action action, float timer, GameObject gameObject, string timerName) {
            this.action = action;
            this.timer = timer;
            isDestroyed = false;
            this.gameObject = gameObject;
            this.timerName = timerName;
        }

        private void Update() {
            if (!isDestroyed) {
                timer -= Time.deltaTime;
                if (timer < 0) {
                    action();
                    DestroySelf();
                }
            }
        }

        private static void InitIfNeeded() {
            if (initGameObject == null) {
                initGameObject = new GameObject("FunctionTimer_InitGameObject");
                timerList = new List<FunctionTimer>();
            }
        }

        private class MonoBehaviourHook : MonoBehaviour {
            public Action onUpdate;

            private void Update() {
                if (onUpdate != null) onUpdate();
            }
        }

        public static FunctionTimer Create(Action action, float timer, string timerName = null) {
            InitIfNeeded();

            string name = timerName != null ? timerName : "FunctionTimer";
            GameObject gameObject = new GameObject(name, typeof(MonoBehaviourHook));
            FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timerName);
            gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

            timerList.Add(functionTimer);
            return functionTimer;
        }

        public static void DestroyTimer(string timerName) {
            InitIfNeeded();
            for (int i = 0; i < timerList.Count; i++) {
                if (timerList[i].timerName == timerName) {
                    timerList[i].DestroySelf();
                    i--;
                }
            }
        }

        public float TimeLeft() {
            return timer;
        }

        private void RemoveTimer(FunctionTimer functionTimer) {
            if (initGameObject == null) InitIfNeeded();
            timerList.Remove(functionTimer);
        }

        private void DestroySelf() {
            isDestroyed = true;
            UnityEngine.Object.Destroy(gameObject);
            RemoveTimer(this);
        }
    }
}
