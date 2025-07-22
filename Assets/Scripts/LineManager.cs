using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    LineRenderer lineRenderer;
    Animator animator;
    Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 endPosition = new Vector3(0.0f, 0.0f, 0.0f);
    float progress;
    GameObject currentStartSocket;
    GameObject currentEndSocket;
    void Start()
    {
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (currentStartSocket != null && currentEndSocket != null)
        {
            progress = animator.GetFloat("progress");
            lineRenderer.SetPosition(0, currentStartSocket.transform.position);
            lineRenderer.SetPosition(1, Vector3.Lerp(currentStartSocket.transform.position, currentEndSocket.transform.position, progress));
        }
        if (currentStartSocket == null && currentEndSocket != null)
        {
            Vector3 startWireEndSocketPosition = currentEndSocket.transform.parent.parent.parent.position + new Vector3(0f, 2.08f, 0f);
            progress = animator.GetFloat("progress");
            lineRenderer.SetPosition(0, currentEndSocket.transform.position);
            lineRenderer.SetPosition(1, Vector3.Lerp(currentEndSocket.transform.position, startWireEndSocketPosition, progress));
        }
    }

    // public void ConnectLine(Vector3 startPos, Vector3 endPos)
    // {
    //     AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
    //     float normalizedTime = info.normalizedTime;
    //     startPosition = startPos;
    //     endPosition = endPos;
    //     lineRenderer.SetPosition(0, startPosition);
    //     if (normalizedTime >= 1)
    //     {
    //         animator.CrossFade("LineEaseToOne", 0.0f, 0, 0f);
    //     }
    //     else
    //     {
    //         animator.CrossFade("LineEaseToOne", 0.0f, 0, 1f - normalizedTime);
    //     }
    // }


    // public void DisconnectLine(Vector3 startPos, Vector3 endPos)
    // {
    //     AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
    //     float normalizedTime = info.normalizedTime;
    //     startPosition = startPos;
    //     endPosition = endPos;
    //     lineRenderer.SetPosition(0, startPosition);
    //     if (normalizedTime >= 1)
    //     {
    //         animator.CrossFade("LineEaseToZero", 0.0f, 0, 0f);
    //     }
    //     else
    //     {
    //         animator.CrossFade("LineEaseToZero", 0.0f, 0, 1f - normalizedTime);
    //     }
    // }

    public void ConnectLine(GameObject startSocket, GameObject endSocket)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;
        currentStartSocket = startSocket;
        currentEndSocket = endSocket;
        // lineRenderer.SetPosition(0, startPosition);
        if (normalizedTime >= 1)
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 0f);
        }
        else
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 1f - normalizedTime);
        }
    }
    public void ConnectLine(GameObject endSocket)
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;
        currentStartSocket = null;
        currentEndSocket = endSocket;
        // lineRenderer.SetPosition(0, startPosition);
        if (normalizedTime >= 1)
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 0f);
        }
        else
        {
            animator.CrossFade("LineEaseToOne", 0.0f, 0, 1f - normalizedTime);
        }
    }


    public void DisconnectLine()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float normalizedTime = info.normalizedTime;
        // lineRenderer.SetPosition(0, startPosition);
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
