using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1408 {
    public class GameDriver : MonoBehaviour {
        private static GameDriver _instance;
        public static GameDriver Instance { get { return _instance; } }

        public GUISkin skin;

        private string _spawnPoint = null;
        public GameObject SpawnPoint { get { return GameObject.Find(_spawnPoint); } }

        private int numWaypoints;
        private List<string> enabledWaypoints;
        private string waypointMsg;
        private GUIStyle waypointMsgStyle;

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
            numWaypoints = GameObject.FindObjectsOfType<Waypoint>().Length;
        }
        public void Start() {
            waypointMsgStyle = new GUIStyle(skin.label);
            waypointMsgStyle.alignment = TextAnchor.MiddleCenter;
            waypointMsgStyle.fontSize = (int)(waypointMsgStyle.fontSize * 1.5f);
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
            if(!enabledWaypoints.Contains(waypoint.name)) {
                enabledWaypoints.Add(waypoint.name);
                StartCoroutine(ShowWaypointCount());
            }
        }
        private IEnumerator ShowWaypointCount() {
            waypointMsg = enabledWaypoints.Count + "/" + numWaypoints + " activated";
            if(enabledWaypoints.Count == numWaypoints) {
                waypointMsg += "\nAll Waypoints Activated!";
                Player.Instance.DisableControl();
            }
            yield return new WaitForSeconds(2f);
            if(enabledWaypoints.Count == numWaypoints) {
                Application.LoadLevel("finished");
            }
            waypointMsg = null;
       }

        public void OnGUI() {
            if(waypointMsg != null) {
                ShadowAndOutline.DrawShadow(new Rect(0, 0, Screen.width, Screen.height),
                    new GUIContent(waypointMsg), waypointMsgStyle,
                    Color.green, Color.black, new Vector2(2, -2));

            }
        }
    }
}