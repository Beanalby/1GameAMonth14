using UnityEngine;
using System.Collections;

public class SinkBumper : MonoBehaviour {
    public Color hitColor = Color.white;
    public AudioClip hitSound;

    private Color original;
    private Material mat;

    private float hitStart = -1, hitDuration = .5f;
    public void Start() {
        mat = GetComponentInChildren<MeshRenderer>().material;
        original = mat.color;
    }

    public void Update() {
        if (hitStart == -1) {
            return;
        }
        float percent = (Time.time - hitStart) / hitDuration;
        if (percent >= 1) {
            mat.color = original;
            hitStart = -1;
        } else {
            mat.color = Color.Lerp(hitColor, original, percent);
        }
    }

    public void OnHit(SinkBall ball) {
        hitStart = Time.time;
        mat.color = hitColor;
        //Debug.Log("ball v=" + ball.rigidbody.velocity.ToString(".000"));
        ball.rigidbody.velocity = 1.1f * ball.rigidbody.velocity;
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
    }
}
