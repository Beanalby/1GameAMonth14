using UnityEngine;
using System.Collections;

public class gbXray : MonoBehaviour {

    public GameObject mask;

    private float maxSize = 5;
    private float spinSpeed = -100;

    private float pulseStart = -1;
    private float pulseDuration = 2;

    public void Start() {
    }
    public void Update() {
        if (pulseStart != -1) {
            float percent = (Time.time - pulseStart) / pulseDuration;
            if(percent >= 1) {
                mask.SetActive(false);
                pulseStart = -1;
            } else {
                mask.transform.position = new Vector3(transform.position.x, transform.position.y, mask.transform.position.z);
                float scale;
                if(percent > .8f) {
                    scale = (1 - ((percent - .8f) * 5)) * maxSize;
                } else if(percent < .2f) {
                    scale = 5 * percent * maxSize;
                } else {
                    scale = maxSize;
                }
                
                mask.transform.localScale = new Vector3(scale, scale, scale);

                Vector3 rot = mask.transform.localRotation.eulerAngles;
                rot.z += spinSpeed * Time.deltaTime;
                mask.transform.localRotation = Quaternion.Euler(rot);
            }
        }
    }

    public void DoXray() {
        if (pulseStart == -1) {
            pulseStart = Time.time;
            mask.SetActive(true);
        }
    }
}
