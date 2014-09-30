using UnityEngine;
using System.Collections;

namespace onegam_1409 {
    public class PlayerAnim : MonoBehaviour {
        public Animator anim;
        public AudioClip jumpSound, landSound, hitSound, dieSound;

        private AudioSource chargeSource;
        private CharacterController2D cc;
        private bool wasGrounded = true;

        public void Start() {
            cc = GetComponent<CharacterController2D>();
            chargeSource = GetComponent<AudioSource>();
        }

        public void Update() {
            bool isGrounded = cc.isGrounded;
            anim.SetBool("IsGrounded", isGrounded);
            if(!wasGrounded && isGrounded && landSound) {
                AudioSource.PlayClipAtPoint(landSound, Camera.main.transform.position);
            }
            wasGrounded = isGrounded;
            anim.SetFloat("hSpeed", Mathf.Abs(cc.velocity.x));
        }

        public void PlayerJumped() {
            anim.SetTrigger("Jump");
            if(jumpSound) {
                AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position);
            }
        }
        public void PlayerDied() {
            anim.SetTrigger("Die");
            if(dieSound) {
                AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
            }
        }
        public void PlayerHit() {
            if(hitSound) {
                AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
            }
        }
        public void SetCharging(bool charging) {
            anim.SetBool("Charging", charging);
            if(charging) {
                chargeSource.Play();
            } else {
                chargeSource.Stop();
            }
        }
    }
}