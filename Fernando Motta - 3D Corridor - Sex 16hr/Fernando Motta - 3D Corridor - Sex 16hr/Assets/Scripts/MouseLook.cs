using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    
    public float lockSpeed = 3;
    private Vector3 rotation = Vector2.zero;
 
    private void LateUpdate()
    {
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -15f, 15f);
        transform.localRotation = Quaternion.Euler(rotation.x * lockSpeed, 0, 0);
    }
}
