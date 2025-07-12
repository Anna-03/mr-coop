using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    LineRenderer lineRenderer;
    Animator animator;
    Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 endPosition = new Vector3(0.0f, 0.0f, 0.0f);
    float progress;
    void Start()
    {
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        progress = animator.GetFloat("progress");

        UpdateLineConnecting();
    }

    void UpdateLineConnecting()
    {
        // Example: animate from start → end based on t
        lineRenderer.SetPosition(1, Vector3.Lerp(startPosition, endPosition, progress));
    }
 

    public void ConnectLine(Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        lineRenderer.SetPosition(0, startPosition);
        animator.Play("LineEase"); // or use trigger
    }
}
