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
        lr.enabled = true;
        mat = lr.material;
    }
    public void Update() {
        if(lr.enabled) {
            UpdateGuide();
        }
    }
    private void UpdateGuide() {
        transform.rotation = Quaternion.identity;
        lr.SetPosition(0, Vector3.zero);
        Vector3 shot;
        float percent;
        sb.GetShotInfo(out shot, out percent);
        lr.SetPosition(1, (shot * percent * guideDistance));
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
