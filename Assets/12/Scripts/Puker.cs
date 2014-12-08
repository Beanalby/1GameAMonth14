﻿using UnityEngine;
using System.Collections;

namespace onegam_1412 {
    [RequireComponent(typeof(LineRenderer))]
    public class Puker : MonoBehaviour {

        [HideInInspector]
        public int direction = 1;

        private LineRenderer lr;
        private Vector2 scrollSpeed = new Vector2(-1f, -.5f);
        private float vomitOffset = 2;

        public void Start() {
            lr = GetComponent<LineRenderer>();
        }

        public void Update() {
            lr.material.SetTextureOffset("_MainTex", Time.time * scrollSpeed);

            // update the coordinates
            Vector3 pos = Vector3.zero;
            if(direction < 0) {
                pos.x -= 2 * (transform.localPosition.x);
            }
            lr.SetPosition(0, pos);
            pos.x += direction * vomitOffset;
            lr.SetPosition(1, pos);
        }
        
    }
}