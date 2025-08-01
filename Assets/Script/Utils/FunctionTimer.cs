using System;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

namespace Game.Utils {
    public class FunctionTimer {
        private static GameObject globalTimerRoot;
        private static List<FunctionTimer> globalTimerList;
        private static GameObject sceneTimerRoot;
        private static List<FunctionTimer> sceneTimerList;
        private static int sceneTimerId = 1;
        private static int globalTimerId = 1;

        private Action action;
        private GameObject gameObject;
        private bool isDestroyed;
        private bool isGlobal;
        private float timer;
        private string timerName;

        private class MonoBehaviourHook : MonoBehaviour {
            public Action onUpdate;

            private void Update() {
                if (onUpdate != null) onUpdate();
            }
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


        FunctionTimer(Action action, float timer, GameObject gameObject, string timerName, bool isGlobal) {
            this.action = action;
            this.timer = timer;
            this.gameObject = gameObject;
            this.timerName = timerName;
            this.isGlobal = isGlobal;
            isDestroyed = false;
        }

        public static FunctionTimer CreateSceneTimer(Action action, float timer, string timerName = null) {
            InitIfNeeded();

            timerName = (timerName ?? "FunctionTimer") + sceneTimerId;
            sceneTimerId++;
            GameObject gameObject = new GameObject(timerName, typeof(MonoBehaviourHook));
            gameObject.transform.SetParent(sceneTimerRoot.transform);

            FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timerName, false);
            gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

            sceneTimerList.Add(functionTimer);
            return functionTimer;
        }

        public static FunctionTimer CreateGlobalTimer(Action action, float timer, string timerName = null) {
            InitIfNeeded();

            timerName = (timerName ?? "FunctionTimer") + globalTimerId;
            globalTimerId++;
            GameObject gameObject = new GameObject(timerName, typeof(MonoBehaviourHook));
            gameObject.transform.SetParent(globalTimerRoot.transform);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);

            FunctionTimer functionTimer = new FunctionTimer(action, timer, gameObject, timerName, true);
            gameObject.GetComponent<MonoBehaviourHook>().onUpdate = functionTimer.Update;

            globalTimerList.Add(functionTimer);
            return functionTimer;
        }

        public static void DestroySceneTimer(string timerName) {
            InitIfNeeded();
            for (int i = 0; i < sceneTimerList.Count; i++) {
                if (sceneTimerList[i].timerName == timerName) {
                    sceneTimerList[i].DestroySelf();
                    i--;
                }
            }
        }

        public static void DestroyGlobalTimer(string timerName) {
            InitIfNeeded();
            for (int i = 0; i < globalTimerList.Count; i++) {
                if (globalTimerList[i].timerName == timerName) {
                    globalTimerList[i].DestroySelf();
                    i--;
                }
            }
        }

        public float TimeLeft() {
            if (gameObject != null)
                return timer;
            else
                return -1;
        }


        private static void InitIfNeeded() {
            if (sceneTimerRoot == null) {
                sceneTimerRoot = new GameObject("FunctionTimer_SceneTimerRoot");
                sceneTimerList = new List<FunctionTimer>();
                sceneTimerId = 1;
            }
            if (globalTimerRoot == null) {
                globalTimerRoot = new GameObject("FunctionTimer_GlobalTimerRoot");
                UnityEngine.Object.DontDestroyOnLoad(globalTimerRoot);
                globalTimerList = new List<FunctionTimer>();
            }
        }

        private void RemoveTimer(FunctionTimer functionTimer) {
            if (functionTimer.isGlobal) {
                if (globalTimerRoot == null) InitIfNeeded();
                globalTimerList.Remove(functionTimer);
                globalTimerId--;
            }
            else {
                if (sceneTimerRoot == null) InitIfNeeded();
                sceneTimerList.Remove(functionTimer);
                sceneTimerId--;
            }
        }

        private void DestroySelf() {
            if (isDestroyed)
                return;

            isDestroyed = true;
            UnityEngine.Object.Destroy(gameObject);
            RemoveTimer(this);
        }
    }
}
