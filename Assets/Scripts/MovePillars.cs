using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus;

public class MovePillarsWithRightJoystick : MonoBehaviour
{
    public float moveSpeed = 1.5f;

    void Update()
    {
        // Input vom rechten Joystick holen
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (input.magnitude > 0.1f)
        {
            Vector3 inputDir = new Vector3(input.x, 0, input.y);

            // Kamera-Forward auf XZ-Ebene (Y=0) projizieren und normalisieren
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Rechts-Vektor von Kamera berechnen
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Bewegung in Kamera-Raum umrechnen
            Vector3 moveDir = cameraRight * inputDir.x + cameraForward * inputDir.z;

            // Bewegung anwenden
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}