using UnityEngine;
using System.Collections;

public class SinkButton : MonoBehaviour {

    public Material buttonPressed;
    public GameObject[] targets;

    private Animator anim;
    private bool isTriggered = false;

    public void Start() {
        anim = GetComponent<Animator>();
    }

    public void OnHit(SinkBall ball) {
        if (!isTriggered) {
            ButtonPressed();
        }
    }
    private void ButtonPressed() {
        isTriggered = true;
        anim.SetTrigger("pressed");
        SkinnedMeshRenderer rend = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] mats = rend.materials;
        mats[1] = buttonPressed;
        rend.materials = mats;
        foreach (GameObject obj in targets) {
            obj.SendMessage("Triggered");
        }
    }

}
