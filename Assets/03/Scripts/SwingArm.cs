using UnityEngine;
using System.Collections;

public class SwingArm : MonoBehaviour {

    public const int maxLength = 5;

    /// <summary>
    /// How much we need to scale y in order to match
    /// the full scale
    /// </summary>
    private const int lengthScale = maxLength * 10;


    
    
    private SwayPlayerSwing swing;
    private SwayPlayer player;

    private SpriteRenderer rend;
    public void Start() {
        rend = GetComponent<SpriteRenderer>();
        swing = transform.parent.GetComponent<SwayPlayerSwing>();
        player = transform.parent.GetComponent<SwayPlayer>();
    }

    public void StartSwing(Rigidbody2D ropePoint) {
        transform.localPosition = swing.SwingPoint;
        rend.enabled = true;
        Update();
    }

    public void Update() {
        if (rend.enabled) {
            Vector3 posBase = transform.position;
            Vector3 posTarget = swing.SwingTarget.transform.position;
            float distance = (posTarget - posBase).magnitude;
            float percent = distance / maxLength;
            transform.localScale = new Vector3(1, lengthScale * percent, 1);
            transform.localRotation = Quaternion.identity;
            float deg = Mathf.Rad2Deg * Mathf.Atan2(player.FacingDir * (posBase.x - posTarget.x), (posTarget.y - posBase.y));
            transform.Rotate(Vector3.forward, deg);
        }
    }

    public void EndSwing() {
        rend.enabled = false;
    }
}

