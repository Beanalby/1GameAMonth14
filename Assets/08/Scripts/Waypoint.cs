using UnityEngine;
using System.Collections;

namespace onegam_1408 {
    public class Waypoint : MonoBehaviour {
        public GameObject spawnPoint;

        public void Start() {
            if(GameDriver.Instance.IsWaypointEnabled(name)) {
                SendMessage("ForceOn");
            } else {
                spawnPoint.SetActive(false);
            }
        }

        public void TurnedOn() {
            GameDriver.Instance.WaypointEnabled(this);
            spawnPoint.SetActive(true);
        }
    }
}