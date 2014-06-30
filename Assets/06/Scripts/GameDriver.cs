using UnityEngine;
using System.Collections;

namespace onegam_1406 {
    public class GameDriver : MonoBehaviour {
        public Texture2D imgReady, imgSet, imgWhack, imgGameOver;

        private Texture2D topImg;

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

        private float timeStart = -1, totalTime = 10f;

        private GUIStyle timeStyle, scoreStyle;
        private float timeLeft {
            get { return Mathf.Max(0, totalTime - (Time.time - timeStart)); }
        }
        public bool IsRunning {
            get { return timeStart != -1 && timeLeft > 0; }
        }
        private int pad = 10;

        public void Start() {

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
            topImg = imgReady;
            yield return new WaitForSeconds(1);
            topImg = imgSet;
            yield return new WaitForSeconds(1);
            topImg = imgWhack;
            yield return new WaitForSeconds(1);
            topImg = null;
            BoardDriver.Instance.SendWave();
            timeStart = Time.time;
        }

        private void EndGame() {
            timeStart = -1;
            topImg = imgGameOver;
            MalletDriver.Instance.GameEnded();
            BoardDriver.Instance.GameEnded();
        }

        public void OnGUI() {
            Vector3 shadowDir = new Vector3(-2, 2, 0);
            Rect scoreRect = new Rect(pad, pad, Screen.width / 2, 100);
            Rect timeRect = new Rect(Screen.width / 2 - pad, pad, Screen.width / 2, 100);
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

            if(topImg != null) {
                GUI.DrawTexture(new Rect(
                    (Screen.width / 2) - topImg.width / 2, 10,
                    topImg.width, topImg.height), topImg);
            }
        }
    }
}