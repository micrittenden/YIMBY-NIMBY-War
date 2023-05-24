using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 90.0f;
    [SerializeField] Vector3 rotationDirection = new Vector3(0, -1, 0);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
    }
}
