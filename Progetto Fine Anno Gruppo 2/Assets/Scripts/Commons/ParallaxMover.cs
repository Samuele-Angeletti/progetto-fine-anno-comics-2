using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMover : MonoBehaviour
{
    [SerializeField,Range(0,1)] private float parallaxEffectMultiplier;
    private Transform cameraPosition;
    private Vector2 lastCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = Camera.main.transform;
        lastCameraPosition = cameraPosition.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 deltaMovement = (Vector2)cameraPosition.position - lastCameraPosition;
        transform.position += (Vector3)deltaMovement * parallaxEffectMultiplier;
        lastCameraPosition = cameraPosition.position;
    }
}
