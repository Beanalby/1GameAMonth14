using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class Coin : MonoBehaviour {

        public float moveSpeed = 0;
        public AudioClip pickupSound;
        public GameObject coinEffect;
        public Transform coinMesh;

        public int value = 1;
        private float offset = 0;
        private float spinSpeed = 90f;
        private int playerLayer;

        public void Start() {
            // start with a random rotation so all coins aren't the same
            offset = Random.Range(-90, 90);
            playerLayer = LayerMask.NameToLayer("Player");
            // 50% chance to flip movement direction
            if(Random.Range(0, 2) == 1) {
                moveSpeed = -moveSpeed;
            }
        }
        void Update() {
            coinMesh.localRotation = Quaternion.Euler(new Vector3(-90, spinSpeed * Time.time + offset, 0));
        }
        public void FixedUpdate() {
            if(moveSpeed != 0 && GameDriver.Instance.IsRunning) {
                // re-set our movespeed every frame to maintain speed
                Vector3 v = rigidbody2D.velocity;
                v.x = moveSpeed;
                // if we're not moving vertically, push it up JUST a little
                // to help it get over corners
                if(v.y == 0) {
                    v.y = .1f;
                }
                rigidbody2D.velocity = v;
            }
        }
        public void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.layer == playerLayer) {
                GameDriver.Instance.CoinPicked(this);
                AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
                GameObject.Instantiate(coinEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}