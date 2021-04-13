using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float RotationSensitivity;
    [SerializeField]
    private float ZoomSensitivity;
    [SerializeField]
    private Transform FollowedObject;
    [SerializeField]
    private float MinDistance;
    private void Update()
    {
        //Zoom
        float deltaZoom = Input.mouseScrollDelta.y;
        Vector3 Direction = (FollowedObject.position - transform.position).normalized;
        transform.position += Direction * deltaZoom * ZoomSensitivity;
        if((FollowedObject.position - transform.position).magnitude < MinDistance)
        {
            transform.position = - Direction * MinDistance;
        }
        // += Vector3.ClampMagnitude((FollowedObject.position - transform.position),MinDistance);
        //Rotation
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");
        transform.RotateAround( FollowedObject.position,transform.up, MouseX * RotationSensitivity);
        transform.RotateAround( FollowedObject.position,transform.right, - MouseY * RotationSensitivity);
    } 
}
