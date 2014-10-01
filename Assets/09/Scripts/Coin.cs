using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class Coin : MonoBehaviour {

        public Transform coinMesh;

        public int value = 1;
        private float offset = 0;
        private float spinSpeed = 90f;

        public void Start() {
            // start with a random rotation so all coins aren't the same
            offset = Random.Range(-90, 90);
        }
        void Update() {
            coinMesh.localRotation = Quaternion.Euler(new Vector3(-90, spinSpeed * Time.time + offset, 0));
        }

        public void OnTriggerEnter2D(Collider2D other) {
            GameDriver.Instance.CoinPicked(this);
            Destroy(gameObject);
        }
    }
}