using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Oculus;

public class MovePillarsWithRightJoystick : MonoBehaviour
{
    public float moveSpeed = 1.5f;

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
        if (inputLeft.magnitude > 0.1f)
        {
            // Bewegung anwenden
            transform.Rotate(0, inputLeft.x * moveSpeed * 30.0f * Time.deltaTime, 0);
        }
    }
}