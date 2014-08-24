using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1408 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance;
        public static GameDriver Instance { get { return _instance; } }

        private string _spawnPoint = null;
        public GameObject SpawnPoint { get { return GameObject.Find(_spawnPoint); } }
        private List<string> enabledWaypoints;

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
            enabledWaypoints = new List<string>();
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

        public bool IsWaypointEnabled(string name) {
            return enabledWaypoints.Contains(name);
        }
        public void WaypointEnabled(Waypoint waypoint) {
            enabledWaypoints.Add(waypoint.name);
        }
    }
}