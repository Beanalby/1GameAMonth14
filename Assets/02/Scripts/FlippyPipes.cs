using UnityEngine;
using System.Collections;

public class FlippyPipes : MonoBehaviour {
    private FlippyDriver driver;
    private bool didPass = false;

    public void Start() {
        driver = GameObject.FindObjectOfType<FlippyDriver>().GetComponent<FlippyDriver>();
    }
    public void OnTriggerEnter2D(Collider2D other) {
        if(!didPass) {
            didPass = true;
            driver.PipePassed(this);
        }
    }
}
