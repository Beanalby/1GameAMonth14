using UnityEngine;
using System.Collections;

public class SwingArm : MonoBehaviour {

    public const int maxLength = 5;

    /// <summary>
    /// How much we need to scale y in order to match
    /// the full scale
    /// </summary>
    private const int lengthScale = maxLength * 10;

    private Transform player;

    private SpriteRenderer rend;
    public void Start() {
        rend = GetComponent<SpriteRenderer>();
        player = transform.parent;
    }

    public void StartSwing(Rigidbody2D ropePoint) {
        transform.parent = ropePoint.transform;
        transform.localPosition = Vector3.zero;
        rend.enabled = true;
        Update();
    }

    public void Update() {
        if (rend.enabled) {
            //float distance = (transform.position - player.position).magnitude;
            //float percent = distance / maxLength;
            //transform.localScale = new Vector3(1, lengthScale * percent);
            //transform.localRotation = Quaternion.LookRotation(player.position - transform.position);
        }
    }

    public void EndSwing() {
        rend.enabled = false;
    }
}

