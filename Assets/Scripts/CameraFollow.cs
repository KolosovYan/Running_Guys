using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Components")]

    [SerializeField] private Transform target;
    [SerializeField] private Transform cashedTransform;

    [Header("Settings")]

    [SerializeField] private float rotationX = 38f;

    private Vector3 offset;

    private void Start()
    {
        offset = cashedTransform.position - target.position;
    }

    private void FixedUpdate()
    {
        cashedTransform.position = target.position + offset;
        cashedTransform.forward = target.forward;
        cashedTransform.rotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
