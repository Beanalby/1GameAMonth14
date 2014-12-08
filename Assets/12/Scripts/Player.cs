﻿using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    public class Player : MonoBehaviour {
        private static Player _instance;
        public static Player Instance {
            get { return _instance; }
        }
        public void Awake() {
            if(_instance != null) {
                Debug.LogError("Can't have 2 players!");
                Debug.Break();
                return;
            }
            _instance = this;
        }

        public float Sickness {
            get { return sickness; }
        }
        public bool IsSick {
            get { return isSick; }
        }

        public Transform sprites;
        public SpriteRenderer body, face;

        public Sprite body1, body2, faceNormal, facePuke;

        float moveStart = -1;
        bool isSick = false;
        private float sickness = 0;
        private float sickDecay = -10f;

        private float animToggle = .5f;
        private float moveSpeed = 75;

        private Rigidbody2D rb;

        public void Start() {
            moveStart = Time.time;
            rb = GetComponent<Rigidbody2D>();
        }

        public void Update() {
            UpdateBody();
            UpdateSickness();
        }

        public void FixedUpdate() {
            if(moveStart != -1) {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                rb.velocity = new Vector2(
                    moveSpeed * Time.deltaTime * h,
                    moveSpeed * Time.deltaTime * v);
                if(h > 0 && sprites.localScale.x < 0
                        || h < 0 && sprites.localScale.x > 0) {
                    sprites.localScale = new Vector3(-sprites.localScale.x, sprites.localScale.y, sprites.localScale.z);
                }
            }
        }
 
        private void UpdateBody() {
            if(moveStart == -1) {
                return;
            }
            float percent = ((Time.time - moveStart) % animToggle) / animToggle;
            if(percent < .5f) {
                body.sprite = body1;
            } else {
                body.sprite = body2;
            }
        }

        private void IncreaseSickness(float amount) {
            sickness = Mathf.Min(100, sickness + amount);
            if(sickness == 100) {
                StartSickness();
            }
        }
        private void UpdateSickness() {
            sickness = Mathf.Max(0, sickness + sickDecay * Time.deltaTime);
            if(sickness == 0) {
                StopSickness();
           }
        }

        void PickupFood(Food food) {
            IncreaseSickness(food.SickValue);
            food.PickedUp();
        }

        void StartSickness() {
            if(isSick) {
                return;
            }
            isSick = true;
            face.sprite = facePuke;
            // todo: start sickly things
        }
        void StopSickness() {
            if(!isSick) {
                return;
            }
            isSick = false;
            face.sprite = faceNormal;
        }

        public void OnTriggerEnter2D(Collider2D other) {
            Debug.Log(name + " collided with " + other.gameObject.name);
            if(other.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
                PickupFood(other.GetComponent<Food>());
            }
        }
    }
}