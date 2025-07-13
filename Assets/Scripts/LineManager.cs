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
        // draws line depending on progress
        lineRenderer.SetPosition(1, Vector3.Lerp(startPosition, endPosition, progress));
    }


    public void ConnectLine(Vector3 startPos, Vector3 endPos)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;
        startPosition = startPos;
        endPosition = endPos;
        lineRenderer.SetPosition(0, startPosition);
        if (normalizedTime >= 1)
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 0f);
        }
        else
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 1f - normalizedTime);
        }
    }


    public void DisconnectLine(Vector3 startPos, Vector3 endPos)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;
        startPosition = startPos;
        endPosition = endPos;
        lineRenderer.SetPosition(0, startPosition);
        if (normalizedTime >= 1)
        {
            animator.CrossFade("LineEaseToZero", 0.0f, 0, 0f);
        }
        else
        {
            animator.CrossFade("LineEaseToZero", 0.0f, 0, 1f - normalizedTime);
        }
    }
}
