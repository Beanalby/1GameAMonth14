using UnityEngine;
using System.Collections;

public class HitEffectRing : MonoBehaviour {
    private float ringStart;
    private float maxSize = 5f;
    private float duration = .5f;

    Material mat;
    Color color;

    public void Start() {
        ringStart = Time.time;
        mat = GetComponent<MeshRenderer>().material;
        color = mat.color;
    }
    public void Update() {
        float percent = (Time.time - ringStart) / duration;
        if(percent >= 1) {
            Destroy(gameObject);
            return;
        }
        float scale = Mathf.Lerp(1, maxSize, percent);
        transform.localScale = new Vector3(scale, scale, scale);
        if(percent >= .5f) {
            color.a = Mathf.Lerp(1, 0, (percent - .5f) * 2);
            mat.color = color;
        }
    }
}
