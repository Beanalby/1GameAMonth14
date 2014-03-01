using UnityEngine;
using System.Collections;

public class FlippyCameraFollow : MonoBehaviour {
    public FlippyPlayer target;
    [HideInInspector]
    public bool IsFollowing = true;

    public void Update() {
        if(target.IsPlaying) {
            Vector3 pos = transform.position;
            pos.x = target.transform.position.x;
            transform.position = pos;
        }
    }
}
