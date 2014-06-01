using UnityEngine;
using System.Collections;

public class SinkWall : MonoBehaviour {
    public AudioClip hitSound;

    public void OnHit(SinkBall ball) {
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
    }
}
