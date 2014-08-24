using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance;
        public static GameDriver Instance { get { return _instance; } }

        private string _spawnPoint = null;
        public GameObject SpawnPoint { get { return GameObject.Find(_spawnPoint); } }

        private float respawnDelay = 3f;

        public void Awake() {
            if(_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
            if(_spawnPoint == null) {
                _spawnPoint = GameObject.FindGameObjectWithTag("InitialSpawn").name;
            }
        }

        public void SetSpawnPoint(GameObject newSpawn) {
            _spawnPoint = newSpawn.name;
        }

        public void PlayerDied() {
            StartCoroutine(_playerDied());
        }

        private IEnumerator _playerDied() {
            yield return new WaitForSeconds(respawnDelay);
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}