using UnityEngine;
using System.Collections;

public class PlayerAnim : MonoBehaviour {
    public Animator anim;

    private CharacterController2D cc;
    private bool wasGrounded = true;

    public void Start() {
        cc = GetComponent<CharacterController2D>();
    }

    public void Update() {
        bool isGrounded = cc.isGrounded;
        if(!wasGrounded && isGrounded) {
            anim.SetTrigger("Land");
        }
        wasGrounded = isGrounded;
        anim.SetFloat("hSpeed", Mathf.Abs(cc.velocity.x));
    }

    public void PlayerJumped() {
        anim.SetTrigger("Jump");
    }
    public void PlayerDied() {
        anim.SetTrigger("Die");
    }
    public void SetCharging(bool charging) {
        anim.SetBool("Charging", charging);
    }
}
