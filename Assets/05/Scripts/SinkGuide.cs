using UnityEngine;
using System.Collections;

public class SinkGuide : MonoBehaviour {
    LineRenderer lr;
    float guideDistance = 3;
    private float scrollSpeed = 2f;
    Material mat;

    SinkBall sb;

    public void Start() {
        sb = GetComponentInParent<SinkBall>();
        lr = GetComponent<LineRenderer>();
        lr.SetVertexCount(2);
        mat = lr.material;
    }
    public void Update() {
        if(lr.enabled) {
            UpdateGuide();
        }
    }
    private void UpdateGuide() {
        lr.SetPosition(0, transform.position);
        Vector3 shot = sb.GetShotInfo();
        lr.SetPosition(1, transform.position + (shot * guideDistance));
        mat.SetTextureOffset("_MainTex", new Vector2(-Time.time * scrollSpeed, 0));
    }

    public void EnableShot() {
        UpdateGuide();
        lr.enabled = true;
    }
    public void DisableShot() {
        lr.enabled = false;
    }
}
