using UnityEngine;
using System.Collections;

namespace onegam_1410 {
    public class Cube : MonoBehaviour {

        public Texture hitTex;
        public AudioClip hitSound;
        public GameObject deathEffect;

        public int value = 1;

        private int layerPlayer, layerGround;

        private bool gotHit = false;

        public void Start() {
            layerPlayer = LayerMask.NameToLayer("Player");
            layerGround = LayerMask.NameToLayer("Ground");
        }
        public void OnCollisionEnter(Collision col) {
            if(!gotHit && col.gameObject.layer == layerPlayer) {
                // move us away from the car
                rigidbody.velocity = 3 * col.relativeVelocity;
                StartCoroutine(GotHit());
            } else if(!gotHit && col.gameObject.layer == layerGround) {
                StartCoroutine(GotHit());
            }
        }

        private IEnumerator GotHit() {
            gotHit = true;
            GameDriver.Instance.CubeHit(this);
            if(hitSound) {
                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            }
            GetComponent<MeshRenderer>().material.SetTexture("_MainTex", hitTex);
            yield return new WaitForSeconds(3);
            GameObject obj = Instantiate(deathEffect) as GameObject;
            obj.transform.position = transform.position;
            Destroy(gameObject);
       }
    }
}