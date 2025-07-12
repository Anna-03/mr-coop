using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus;

public class MovePillarsWithRightJoystick : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float radius = 1f;
    void Start()
    {
        placePillars();
    }
    void Update()
    {
        // InputRight vom rechten Joystick holen
        Vector2 inputRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        Vector2 inputLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        if (inputRight.magnitude > 0.1f)
        {
            Vector3 inputRightDir = new Vector3(inputRight.x, 0, inputRight.y);

            // Kamera-Forward auf XZ-Ebene (Y=0) projizieren und normalisieren
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Rechts-Vektor von Kamera berechnen
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Bewegung in Kamera-Raum umrechnen
            Vector3 moveDir = cameraRight * inputRightDir.x + cameraForward * inputRightDir.z;

            // Bewegung anwenden
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        if (Mathf.Abs(inputLeft.x) > 0.7f)
        {
            // Bewegung anwenden
            transform.Rotate(0, inputLeft.x * moveSpeed * 30.0f * Time.deltaTime, 0);
        }
        if (Mathf.Abs(inputLeft.y) > 0.7f)
        {
            // Bewegung anwenden
            radius += inputLeft.y * moveSpeed * Time.deltaTime / 2.0f;
            placePillars();
        }

    }
    void placePillars()
    {
                int childCount = transform.childCount;
        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate angle in radians
            float angle = i * angleStep * Mathf.Deg2Rad;

            // Position on circle
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );

            child.localPosition = position;

            // Make the pillar face the center (0,0,0 relative to parent)
            child.LookAt(transform.position);
        }
    }
}