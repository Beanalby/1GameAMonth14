using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance = null;
        public static GameDriver Instance {
            get {
                if(_instance == null) {
                    Debug.LogError("Accessing GameDriver instance before Awake");
                    Debug.Break();
                    return null;
                }
                return _instance;
            }
        }
        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Two GameDrivers, that shouldn't happen.");
                Debug.Break();
                return;
            }
            _instance = this;
        }


        public GUISkin skin;
        private string msg = null;

        private float timeStart = -1, totalTime = 10f;

        private GUIStyle msgStyle, timeStyle, scoreStyle;
        private float timeLeft {
            get { return Mathf.Max(0, totalTime - (Time.time - timeStart)); }
        }
        public bool IsRunning {
            get { return timeStart != -1 && timeLeft > 0; }
        }
        private int pad = 10;

        public void Start() {
            msgStyle = new GUIStyle(skin.label);
            msgStyle.alignment = TextAnchor.UpperCenter;
            msgStyle.fontSize *= 2;

            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.UpperRight;
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperLeft;
            StartCoroutine(Intro());
        }

        public void Update() {
            if(timeStart != -1 && timeLeft <= 0) {
                EndGame();
            }
        }
        private IEnumerator Intro() {
            yield return new WaitForSeconds(1);
            msg = "Ready...";
            yield return new WaitForSeconds(1);
            msg = "Set...";
            yield return new WaitForSeconds(1);
            msg = "WHACK!";
            yield return new WaitForSeconds(1);
            msg = null;
            BoardDriver.Instance.SendWave();
            timeStart = Time.time;
        }

        private void EndGame() {
            timeStart = -1;
            msg = "END!!!";
            MalletDriver.Instance.GameEnded();
            BoardDriver.Instance.GameEnded();
        }

        public void OnGUI() {
            Vector3 shadowDir = new Vector3(-2, 2, 0);
            Rect scoreRect = new Rect(pad, pad, Screen.width / 2, 100);
            Rect timeRect = new Rect(Screen.width / 2 - pad, pad, Screen.width / 2, 100);
            Rect msgRect = new Rect(0, 10, Screen.width, 100);
            GUIContent content = new GUIContent();

            content.text = "Score: 12";
            ShadowAndOutline.DrawShadow(scoreRect, content, scoreStyle,
                Color.white, Color.black, shadowDir);
            content.text = "Time: ";
            if(timeStart == -1) {
                content.text += "____";
            } else {
                content.text += timeLeft.ToString("00.0");
            }
            ShadowAndOutline.DrawShadow(timeRect, content, timeStyle,
                Color.white, Color.black, shadowDir);
            
            if(msg != null) {
                content.text = msg;
                ShadowAndOutline.DrawShadow(msgRect, content, msgStyle,
                    Color.white, Color.black, shadowDir);
            }
        }
    }
}