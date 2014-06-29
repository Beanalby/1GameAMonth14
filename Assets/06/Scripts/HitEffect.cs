using UnityEngine;
using System.Collections;

public class HitEffect : MonoBehaviour {
    public GameObject HitEffectRing;
    public Light HitEffectLight;

    private float duration = .8f;
    private float numRings = 2;

    private float start;
    
    private float ringDelay = .2f;

    private float baseIntensity;

    public void Start() {
        start = Time.time;
        StartCoroutine(MakeRings());
        baseIntensity = HitEffectLight.intensity;
    }

    public void Update() {
        float percent = (Time.time - start) / duration;
        if(percent >= 1) {
            Destroy(gameObject);
            return;
        }
        if(percent >= .75f) {
            HitEffectLight.intensity = Mathf.Lerp(baseIntensity, 0,
                (percent - .75f) * 4);
        }
    }

    private IEnumerator MakeRings() {
        while(numRings-- > 0) {
            MakeRing();
            yield return new WaitForSeconds(ringDelay);
        }
    }
    void MakeRing() {
        GameObject obj = Instantiate(HitEffectRing) as GameObject;
        obj.transform.localPosition += transform.position;
    }
}
