using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class GameDriver : MonoBehaviour {
        public static GameDriver Instance {
            get { return _instance; }
        }
        private static GameDriver _instance = null;
        public void Awake() {
            if (_instance != null) {
                Debug.LogError("Can't have two GameDrivers");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public GUISkin skin;
        public GameObject foodPrefab, targetPrefab;

        private int foodCount = 4, targetCount = 4;
        private Vector2 foodRange = new Vector2(3,2);
        private Vector2 targetRange = new Vector2(5, 4);
        private bool isRunning = true;

        private float timeStart = -1, totalTime = 30f;
        public float TimeLeft {
            get {
                if (timeStart == -1) {
                    return timeStart;
                } else {
                    return Mathf.Max(0, totalTime - (Time.time - timeStart));
                }
            }
        }

        private GUIStyle sickStyle, scoreStyle, msgStyle;
        private int score = 0;

        private float foodSpawnDelay = 2f;

        public void Start() {
            sickStyle = new GUIStyle(skin.label);
            sickStyle.alignment = TextAnchor.UpperLeft;
            scoreStyle = new GUIStyle(skin.label);
            scoreStyle.alignment = TextAnchor.UpperRight;
            msgStyle = new GUIStyle(skin.label);
            msgStyle.alignment = TextAnchor.UpperCenter;

            // load up the scene with starting food and people
            for (int i = 0; i < foodCount; i++) {
                StartCoroutine(SpawnFood(true));
            }
            for(int i=0; i < targetCount; i++) {
                SpawnTarget();
            }
            timeStart = Time.time;
        }

        public void Update() {
            if (timeStart != -1 && isRunning && TimeLeft <= 0) {
                GameOver();
            }
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect sickRect = new Rect(10, 10, 200, 50);
            Rect scoreRect = new Rect(Screen.width - 210, 10, 200, 50);
            Rect timeRect = new Rect(Screen.width - 210, 40, 200, 50);
            Rect msgRect = new Rect(0, 300, Screen.width, 200);
            Rect againRect = new Rect(Screen.width/2 - 100, 400, 200, 100);
            GUIContent content;

            content = new GUIContent("Sickness: " + Player.Instance.Sickness.ToString("N0"));
            ShadowAndOutline.DrawShadow(sickRect, content, sickStyle,
                Color.white, Color.black, new Vector2(-3, -3));

            content = new GUIContent("Score: " + score);
            ShadowAndOutline.DrawShadow(scoreRect, content, scoreStyle,
                Color.white, Color.black, new Vector2(-3, -3));

            content = new GUIContent("Time: " + TimeLeft.ToString("N1"));
            ShadowAndOutline.DrawShadow(timeRect, content, scoreStyle,
                Color.white, Color.black, new Vector2(-3, -3));

            if (!isRunning) {
                content = new GUIContent("Time up!  You successfully made " + score + " people miserable like you.");
                ShadowAndOutline.DrawShadow(msgRect, content, msgStyle,
                    Color.white, Color.black, new Vector2(-3, -3));
                if (GUI.Button(againRect, "Play Again")) {
                    Application.LoadLevel("title");
                }
            }
        }

        private void GameOver() {
            //kill the player, food, and targets
            isRunning = false;
            Player.Instance.RemovePlayer();
            foreach (Target t in GameObject.FindObjectsOfType<Target>()) {
                t.RemoveTarget();
            }
            foreach (Food f in GameObject.FindObjectsOfType<Food>()) {
                f.RemoveFood();
            }
        }

        public void FoodEaten() {
            if (isRunning) {
                StartCoroutine(SpawnFood(false));
            }
        }
        private IEnumerator SpawnFood(bool immediate) {
            if (!immediate) {
                yield return new WaitForSeconds(foodSpawnDelay);
            }
            if (!isRunning) {
                yield break;
            }
            Vector3 pos = new Vector3(
                Random.Range((int)-foodRange.x, (int)foodRange.x),
                Random.Range((int)-foodRange.y, (int)foodRange.y),
                0);
            Instantiate(foodPrefab, pos, Quaternion.identity);
        }

        private void SpawnTarget() {
            // spawn a target beyond the outside border
            Vector3 pos, v;
            float x, y;
            switch (Random.Range(0,4)) {
                case 0:
                    // left side
                    y = Random.Range(-targetRange.y / 2, targetRange.y / 2);
                    pos = new Vector3(-targetRange.x, y, 0);
                    v = new Vector2(targetRange.x / 2, 
                        Random.Range(-targetRange.y / 4, targetRange.y / 4));
                    break;
                case 1:
                    // right side
                    y = Random.Range(-targetRange.y / 2, targetRange.y / 2);
                    pos = new Vector3(targetRange.x, y, 0);
                    v = new Vector2(-targetRange.x / 2, 
                        Random.Range(-targetRange.y / 4, targetRange.y / 4));
                    break;
                case 2:
                    // top side
                    x = Random.Range(-targetRange.x / 2, targetRange.x / 2);
                    pos = new Vector3(x, targetRange.y, 0);
                    v = new Vector2(Random.Range(-targetRange.x / 4, targetRange.x / 4),
                        -targetRange.y / 2);
                    break;
                case 3:
                    // top side
                    x = Random.Range(-targetRange.x / 2, targetRange.x / 2);
                    pos = new Vector3(x, -targetRange.y, 0);
                    v = new Vector2(Random.Range(-targetRange.x / 4, targetRange.x / 4),
                        targetRange.y / 2);
                    break;
                default:
                    Debug.LogError("WHAT!?!?!  Invalid random side.");
                    return;
            }
            GameObject obj = (GameObject)Instantiate(targetPrefab, pos, Quaternion.identity);
            obj.rigidbody2D.velocity = v;
        }

        public void TargetPuked() {
            score++;
        }
        public void TargetRemoved() {
            if (isRunning) {
                SpawnTarget();
            }
        }
    }
}