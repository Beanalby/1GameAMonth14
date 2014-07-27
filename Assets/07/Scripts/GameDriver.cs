using UnityEngine;
using System.Collections;

namespace onegam_1407 {
    public class GameDriver : MonoBehaviour {

        private static GameDriver _instance;
        public static GameDriver Instance {
            get { return _instance; }
        }

        public GUISkin skin;
        private float timeTotal=30f, timeStart=-1;
        public float TimeLeft {
            get {
                if(timeStart == -1) {
                    return timeTotal;
                } else {
                    return Mathf.Max(0, timeTotal - (Time.time - timeStart));
                }
            }
        }
        private GUIStyle scoreStyle, timeStyle;

        private int score;

        private bool _isRunning;
        public bool IsRunning {
            get { return _isRunning; }
        }

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Other GameDriver " + _instance.name + " exists");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public void Start() {
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperLeft;
            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.UpperRight;
        }

        public void Update() {
            if(!IsRunning && Input.GetKeyDown(KeyCode.R)) {
                StartGame();
            }
            if(IsRunning && TimeLeft <= 0) {
                EndGame();
            }
        }

        private void StartGame() {
            _isRunning = true;
            timeStart = Time.time;
        }
        private void EndGame() {
            _isRunning = false;
        }
        public void CargoDelivered(Cargo cargo) {
            score += 100;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Vector3 shadowDir = new Vector3(-2, 2, 0);
            Rect scoreRect = new Rect(10, 10, Screen.width / 2, 100);
            Rect timeRect = new Rect(Screen.width / 2, 10, Screen.width / 2 - 10, 100);
            GUIContent scoreData = new GUIContent("Score: $" + score);
            GUIContent timeData = new GUIContent("Time: " + TimeLeft.ToString(".0"));

            ShadowAndOutline.DrawShadow(scoreRect, scoreData,
                scoreStyle, Color.white, Color.black, shadowDir);
            ShadowAndOutline.DrawShadow(timeRect, timeData,
                timeStyle, Color.white, Color.black, shadowDir);
        }
    }
}