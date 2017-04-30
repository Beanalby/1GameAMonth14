using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1409 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance = null;
        public static GameDriver Instance { get { return _instance; } }

        public GUISkin skin;
        public GameObject coinYellow, coinRed;
        private float totalTime = 30;

        public bool IsRunning {
            get {
                return timeStart != -1 && TimeLeft != 0;
            }
        }
        
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
        private int score=0;
        private float redChance = .2f;
        public int Score {
            get { return score; }
        }

        private CoinSpawn[] spawns;
        private Queue<CoinSpawn> lastSpawns;
        private static int simultaneousCoins = 4;
        private static int spawnHistory = 6;
        private string msg;
        private GUIStyle msgStyle;

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have multiple GameDrivers");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        // Use this for initialization
        void Start() {
            msgStyle = new GUIStyle(skin.label);
            msgStyle.fontSize *= 2;
            msgStyle.alignment = TextAnchor.UpperCenter;

            spawns = GameObject.FindObjectsOfType<CoinSpawn>();
            lastSpawns = new Queue<CoinSpawn>();
            while(lastSpawns.Count < simultaneousCoins) {
                SpawnCoin(false);
            }
            StartCoroutine(Intro());
            ShowControls sc = ShowControls.CreateDocked(new ControlItem[] {
                new ControlItem("Move left and right", new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow }),
                new ControlItem("Jump", KeyCode.Space)
            });
            sc.showDuration = 4;
            sc.position = ShowControlPosition.Bottom;
            sc.Show();
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect scoreRect = new Rect(10, -10, 300, 100);
            Rect timeRect = new Rect(Screen.width - 260, -10, 250, 100);
            Rect msgRect = new Rect(0, 10, Screen.width, 200);
            if(TimeLeft == 0) {
                msg = "Game Over";
                Rect retryButton = new Rect((Screen.width/2) - 200, Screen.height / 2, 400, 100);
                if(GUI.Button(retryButton, "Play Again")) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }

            ShadowAndOutline.DrawShadow(scoreRect, new GUIContent("Score: " + score),
                skin.label, Color.white, Color.black, new Vector2(3, 3));
            ShadowAndOutline.DrawShadow(timeRect, new GUIContent("Time: " + TimeLeft.ToString("F1")),
                skin.label, Color.white, Color.black, new Vector2(3, 3));
            if(msg != null) {
                ShadowAndOutline.DrawShadow(msgRect, new GUIContent(msg),
                    msgStyle, Color.white, Color.black, new Vector2(6, 6));
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
            yield return new WaitForSeconds(1);
            msg = null;
        }
        private void SpawnCoin(bool allowRed=true) {
            CoinSpawn spawn = GetCoinSpawn();
            GameObject prefab;
            if(allowRed && spawn.canSpawnRed && Random.Range(0f, 1f) < redChance) {
                prefab = coinRed;
            } else {
                prefab = coinYellow;
            }
            GameObject.Instantiate(prefab, spawn.transform.position, Quaternion.identity);
        }

        private CoinSpawn GetCoinSpawn() {
            CoinSpawn spawn;
            // find a spawn location we didn't recently use
            while(true) {
                spawn = spawns[Random.Range(0, spawns.Length)];
                if(!lastSpawns.Contains(spawn)) {
                    break;
                }
            }
            lastSpawns.Enqueue(spawn);
            if(lastSpawns.Count > spawnHistory) {
                lastSpawns.Dequeue();
            }
            return spawn;
        }
        public void CoinPicked(Coin coin) {
            score += coin.value;
            SpawnCoin();
        }

    }
}