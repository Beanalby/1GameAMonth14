using UnityEngine;
using System.Collections;

public class SinkGuide : MonoBehaviour {
    LineRenderer lr;
    float maxGuideDist = 3;
    private float scrollSpeed = 2f;
    Camera cam;
    Material mat;

    public void Start() {
        cam = Camera.main;
        lr = GetComponentInChildren<LineRenderer>();
        lr.SetVertexCount(2);
        mat = lr.material;

    }
    public void Update() {
        UpdateGuide();
    }
    private void UpdateGuide() {
        lr.SetPosition(0, transform.position);

        // Find the cursor's point projected onto the ball's plane
        Plane ballPlane = new Plane(Vector3.up, transform.position);
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
        float dist;
        if (!ballPlane.Raycast(mouseRay, out dist)) {
            Debug.LogError("Mouse ray doesn't intersect ball plane!");
            Debug.Break();
            return;
        }
        Vector3 pos = mouseRay.GetPoint(dist);
        // note how far away the curor is from the ball
        float mouseDist = (pos - transform.position).magnitude;
        float lineDist = Mathf.Min(maxGuideDist, mouseDist);
        Vector3 dir = (pos - transform.position).normalized;
        lr.SetPosition(1, transform.position - (dir * lineDist));
        mat.SetTextureOffset("_MainTex", new Vector2(-Time.time * scrollSpeed, 0));
    }
}
