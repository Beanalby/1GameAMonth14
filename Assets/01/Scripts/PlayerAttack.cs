using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    private Animator animator;
    private SphereCollider attackArea;
    private PlayerController pc;
    private float lastAttack = -1f, cooldown = 1.0f;
    private int attackableMask;


    void Start () {
        attackArea = transform.Find("AttackArea").GetComponent<SphereCollider>();
        pc = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        attackableMask = 1 << LayerMask.NameToLayer("Attackable");
    }
    
    // Update is called once per frame
    void Update () {
        HandleAttacking();
    }
    private void HandleAttacking() {
        if(!pc.CanControl) {
            return;
        }
        if(lastAttack + cooldown > Time.time) {
            return;
        }
        if(Input.GetButtonDown("Jump")) {
            Debug.Log("Attacking!");
            lastAttack = Time.time;
            animator.SetTrigger("Attack");
            // see if anything's inside the attack area
            Vector3 pos = attackArea.transform.position;
            float radius = attackArea.radius;
            Collider[] hits = Physics.OverlapSphere(pos, radius, attackableMask);
            foreach(Collider hit in hits) {
                hit.SendMessage("GotHit", this);
            }
        }
    }
}
