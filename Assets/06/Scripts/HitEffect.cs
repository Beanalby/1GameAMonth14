using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {
    public GameObject HitEffectRing;

    private float duration = 1f;

    private float start;
    
    private float ringDelay = .2f;

    public void Start() {
        start = Time.time;
        StartCoroutine(MakeRings());
    }

    public void Update() {
        float percent = (Time.time - start) / duration;
        if(percent >= 1) {
            Destroy(gameObject);
            return;
        }
    }

    private IEnumerator MakeRings() {
        while(true) {
            MakeRing();
            yield return new WaitForSeconds(ringDelay);
        }
    }
    void MakeRing() {
        GameObject obj = Instantiate(HitEffectRing) as GameObject;
        obj.transform.localPosition += transform.position;
    }
}
