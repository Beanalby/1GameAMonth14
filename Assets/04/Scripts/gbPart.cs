using UnityEngine;
using System.Collections;

public class gbPart : MonoBehaviour {
    public AudioClip pickupSound;
    public void PickedUp() {
        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        Destroy(gameObject);
    }
}
