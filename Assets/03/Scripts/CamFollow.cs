using UnityEngine;
using System.Collections;



public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float smoothDampTime = 0.2f;
    [HideInInspector]
    public new Transform transform;
    public Vector3 cameraOffset;
    public bool useFixedUpdate = true;

    public Vector2 MinPosition, maxPosition;

    private CharacterController2D _playerController;
    private Vector3 _smoothDampVelocity;
    
    
    void Awake()
    {
        transform = gameObject.transform;
        _playerController = target.GetComponent<CharacterController2D>();
    }
    
    
    void LateUpdate()
    {
        if( !useFixedUpdate )
            updateCameraPosition();
    }


    void FixedUpdate()
    {
        if( useFixedUpdate )
            updateCameraPosition();
    }


    void updateCameraPosition()
    {
        if(target == null) {
            return;
        }
        Vector3 pos;
        if( _playerController == null)
        {
            pos = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
            return;
        }
        
        if( _playerController.velocity.x > 0 )
        {
            pos = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;
            pos = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
        }
        pos.x = Mathf.Min(Mathf.Max(pos.x, MinPosition.x), maxPosition.x);
        pos.y = Mathf.Min(Mathf.Max(pos.y, MinPosition.y), maxPosition.y);
        transform.position = pos;
    }
}
