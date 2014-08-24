using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class StageDriver : MonoBehaviour {
        private static StageDriver _instance;
        public static StageDriver Instance { get { return _instance; } }

        public Player playerPrefab;

        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have two StageDrivers");
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        [HideInInspector]
        public GameObject SpawnPoint;

        public void Respawn() {
            GameObject player = (GameObject)Instantiate(playerPrefab,
                SpawnPoint.transform.position, Quaternion.identity);
        }
    }
}