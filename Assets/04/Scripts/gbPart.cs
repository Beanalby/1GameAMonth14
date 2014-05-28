using UnityEngine;
using System.Collections;

public class gbPart : MonoBehaviour {
    public AudioClip pickupSound;
    public GameObject otherShip;

    private float zoomStart = -1;
    private float zoomDuration = 1f;
    private float maxZoom = 2;

    public void Update() {
        if(zoomStart == -1) {
            return;
        }
        float percent = (Time.time - zoomStart) / zoomDuration;
        if(percent >= 1) {
            Destroy(gameObject);
        } else {
            float scale;
            if(percent < .33f) {
                scale = Mathf.Lerp(1, maxZoom, percent / .33f);
            } else {
                scale = Mathf.Lerp(maxZoom, 0, (percent - .33f) / .66f);
            }
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    public void PickedUp() {
        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        otherShip.SetActive(true);
        zoomStart = Time.time;
    }
}
