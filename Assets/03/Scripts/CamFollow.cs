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

    public float minHeight = -100;

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
        if( _playerController == null || !_playerController.enabled)
        {
            transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
            return;
        }
        
        if( _playerController.velocity.x > 0 )
        {
            transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;
            transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
        }
        if(transform.position.y < minHeight) {
            transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
        }
    }
    
}
