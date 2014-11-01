using UnityEngine;
using System.Collections;

namespace onegam_1410 {
    public class GameDriver : MonoBehaviour {
        public GUISkin skin;

        private int score;
        private float totalTime = 10;
        private float timeStart = -1;
        public float TimeLeft {
            get {
                if(timeStart == -1) {
                    return totalTime;
                } else {
                    return Mathf.Max(0, timeStart + totalTime - Time.time);
                }
            }
        }
        private bool isRunning = false;

        public bool IsRunning { get { return isRunning; } }

        private string msg = null;
        private GUIStyle msgStyle, timeStyle;
        private CarController player;

        static private GameDriver _instance = null;
        static public GameDriver Instance { get { return _instance; } }

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("GameDriver already exists");
                Destroy(gameObject);
            }
            _instance = this;
        }

        public void Start() {
            player = GameObject.FindObjectOfType<CarController>();
            player.Immobilize();
            msgStyle = new GUIStyle(skin.label);
            msgStyle.alignment = TextAnchor.UpperCenter;
            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.UpperRight;
            StartCoroutine(Intro());
        }

        public void Update() {
            if(isRunning && timeStart != -1 && TimeLeft == 0) {
                isRunning = false;
                player.Immobilize();
            }
        }
        private IEnumerator Intro() {
            yield return new WaitForSeconds(1);
            msg = "READY";
            yield return new WaitForSeconds(1);
            msg = "SET";
            yield return new WaitForSeconds(1);
            msg = "GO!";
            timeStart = Time.time;
            isRunning = true;
            player.Reset();
            yield return new WaitForSeconds(1);
            msg = null;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect scoreRect = new Rect(10, 10, 300, 100);
            Rect timeRect = new Rect(Screen.width - 310, 10, 300, 100);
            GUIContent scoreContent = new GUIContent("Score: " + score.ToString());
            GUIContent timeContent = new GUIContent("Time: " + TimeLeft.ToString("F1"));

            ShadowAndOutline.DrawShadow(scoreRect, scoreContent,
                skin.label,  Color.white, Color.black, new Vector2(3, -3));
            ShadowAndOutline.DrawShadow(timeRect, timeContent,
                timeStyle, Color.white, Color.black, new Vector2(3, 3));

            if(TimeLeft == 0) {
                msg = "Game Over";
                Rect retryButton = new Rect((Screen.width / 2) - 200, Screen.height / 2, 400, 100);
                if(GUI.Button(retryButton, "Play Again")) {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }
            if(msg != null) {
                Rect msgRect = new Rect(0, 10, Screen.width, 200);
                ShadowAndOutline.DrawShadow(msgRect, new GUIContent(msg),
                    msgStyle, Color.white, Color.black, new Vector2(6, 6));
            }

        }

        public void CubeHit(Cube cube) {
             score += cube.value;
        }
    }
}