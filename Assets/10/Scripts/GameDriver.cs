using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace onegam_1410 {
    public class GameDriver : MonoBehaviour {
        public GUISkin skin;
        public GameObject[] CubeGroups;
        public SpawnPoint[] startingPoints;

        private int score;
        private float totalTime = 60;
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

        private float CubeSpawnDelay = 3f;
        private SpawnPoint[] SpawnPoints;
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
            SpawnPoints = GameObject.FindObjectsOfType<SpawnPoint>();
            msgStyle = new GUIStyle(skin.label);
            msgStyle.alignment = TextAnchor.UpperCenter;
            timeStyle = new GUIStyle(skin.label);
            timeStyle.alignment = TextAnchor.UpperRight;
            StartCoroutine(Intro());
            // spawn some cubes in the starting points so they can be seen
            SpawnCubes(startingPoints);
            SpawnCubes(startingPoints);
            SpawnCubes(SpawnPoints);
            SpawnCubes(SpawnPoints);
        }

        public void Update() {
            if(isRunning && timeStart != -1 && TimeLeft == 0) {
                isRunning = false;
                player.Immobilize();
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            if(!isRunning && timeStart != -1 && Input.GetButtonDown("Jump")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            StartCoroutine(SpawnCubeLoop());
        }

        private IEnumerator SpawnCubeLoop() {
            while(true) {
                if(!IsRunning) {
                    yield break;
                }
                SpawnCubes(SpawnPoints);
                yield return new WaitForSeconds(CubeSpawnDelay);
            }

        }

        private void SpawnCubes(SpawnPoint[] points) {
            SpawnPoint point = GetSpawnPoint(points);
            Instantiate(CubeGroups[Random.Range(0, CubeGroups.Length)],
                point.transform.position, Quaternion.identity);
            // find a spawn point that doesn't have anything there
        }

        private SpawnPoint GetSpawnPoint(SpawnPoint[] points) {
            while(true) {
                SpawnPoint point = points[Random.Range(0, points.Length)];
                Vector3 pos = point.transform.position;
                pos.y += 4.01f;
                if(Physics.OverlapSphere(pos, 4).Length == 0) {
                    return point;
                }
            }
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
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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