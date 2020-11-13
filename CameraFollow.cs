using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public variables
    public Transform toFollow;
    [Range(-10, 0)] public float cameraZOffset;

    // Update is called once per frame
    void Update()
    {
        // Move the camera to the position of the object to follow
        transform.position = new Vector3(toFollow.position.x, toFollow.position.y, cameraZOffset);
    }
}
