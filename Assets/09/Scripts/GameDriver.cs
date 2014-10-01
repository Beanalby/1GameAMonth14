using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1409 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance = null;
        public static GameDriver Instance { get { return _instance; } }

        public GUISkin skin;
        public GameObject coinYellow, coinRed;

        private int score=0;
        private float redChance = .2f;
        public int Score {
            get { return score; }
        }

        private CoinSpawn[] spawns;
        private Queue<CoinSpawn> lastSpawns;
        private static int simultaneousCoins = 4;
        private static int spawnHistory = 6;

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
            spawns = GameObject.FindObjectsOfType<CoinSpawn>();
            lastSpawns = new Queue<CoinSpawn>();
            while(lastSpawns.Count < simultaneousCoins) {
                SpawnCoin(false);
            }
        }

        public void OnGUI() {
            GUI.skin = skin;
            Rect scoreRect = new Rect(10, -10, 300, 100);
            ShadowAndOutline.DrawShadow(scoreRect, new GUIContent("Score: " + score),
                skin.label, Color.white, Color.black, new Vector2(3, 3));
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