using UnityEngine;
using System.Collections;

namespace onegam_1407 {

    public class GameDriver : MonoBehaviour {

        private static GameDriver _instance;
        public static GameDriver Instance {
            get { return _instance; }
        }

        public GUISkin skin;
        public int activePickups;
        public float cargoValue = 100;
        public float timeTotal = 45f;

        private float timeStart=-1;
        public float TimeLeft {
            get {
                if(timeStart == -1) {
                    return timeTotal;
                } else {
                    return Mathf.Max(0, timeTotal - (Time.time - timeStart));
                }
            }
        }
        private string titleMsg = null;
        private GUIStyle scoreStyle, timeStyle, titleStyle;
        private int score;
        private CargoPickup[] pickups;

        private bool isStarting = true;
        private bool isRunning = false;
        public bool IsRunning {
            get { return isRunning; }
        }
        public bool CanControl {
            get { return isStarting || IsRunning; }
        }
        private bool canRestart = false;

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Other GameDriver " + _instance.name + " exists");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public void Start() {
            pickups = FindObjectsOfType<CargoPickup>();
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperRight;
            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.UpperLeft;
            titleStyle = new GUIStyle(skin.label);
            titleStyle.fontSize = (int)(1.5f * titleStyle.fontSize);
            titleStyle.alignment = TextAnchor.UpperLeft;
            while (activePickups != 0) {
                SpawnRandomCargo(true);
                activePickups--;
            }
            StartCoroutine(StartGame());
        }

        public void Update() {
            if(IsRunning && TimeLeft <= 0) {
                StartCoroutine(EndGame());
            }
            if(canRestart && Input.GetKeyDown(KeyCode.Space)) {
                Application.LoadLevel("levelChooser");
            }
        }

        
        private IEnumerator StartGame() {
            yield return new WaitForSeconds(1f);
            titleMsg = "Ready...";
            yield return new WaitForSeconds(1f);
            titleMsg = "Set...";
            yield return new WaitForSeconds(1f);
            titleMsg = "Go!";
            isStarting = false;
            isRunning = true;
            timeStart = Time.time;
            yield return new WaitForSeconds(1f);
            titleMsg = null;
        }
        private IEnumerator EndGame() {
            isRunning = false;
            titleMsg = "TIME UP!";
            yield return new WaitForSeconds(1f);
            titleMsg = "TIME UP!\nSpace to play again";
            canRestart = true;
        }
        public void CargoPicked(Cargo cargo) {
            SpawnRandomCargo(false);
        }

        private void SpawnRandomCargo(bool now) {
            while (true) {
                CargoPickup p = pickups[Random.Range(0, pickups.Length)];
                if (p.CanSpawn()) {
                    if (now) {
                        p.SpawnCargo();
                    } else {
                        p.SpawnAfterDelay();
                    }
                    return;
                }
            }
        }

        public void CargoDelivered(Cargo cargo) {
            score += 100;
        }

        public void OnGUI() {
            GUI.skin = skin;
            Vector3 shadowDir = new Vector3(-2, 2, 0);
            Rect timeRect = new Rect(10, 10, Screen.width / 2, 100);
            Rect scoreRect = new Rect(Screen.width / 2, 10, Screen.width / 2 - 10, 100);
            Rect titleRect = new Rect(0, 75, Screen.width, 150);
            GUIContent scoreData = new GUIContent("Score: $" + score);
            GUIContent timeData = new GUIContent("Time: " + TimeLeft.ToString(".0"));

            ShadowAndOutline.DrawShadow(scoreRect, scoreData,
                scoreStyle, Color.white, Color.black, shadowDir);
            ShadowAndOutline.DrawShadow(timeRect, timeData,
                timeStyle, Color.white, Color.black, shadowDir);
            if(titleMsg != null) {
                GUIContent titleContent = new GUIContent(titleMsg);
                ShadowAndOutline.DrawShadow(titleRect, titleContent,
                    titleStyle, Color.white, Color.black, shadowDir);
            }
       }
    }
}