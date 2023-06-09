using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform cameraJig;
    private float rotateSpeed = 120;

    // Rotate the camera around the player
    void LateUpdate()
    {
        if (Input.GetKey (KeyCode.E))
        {
            transform.RotateAround(cameraJig.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKey (KeyCode.Q))
        {
            transform.RotateAround(cameraJig.position, -Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
