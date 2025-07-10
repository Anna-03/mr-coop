using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour
{
    GameObject[] linePath;
    private LineRenderer lineRenderer;
    bool pathFound;
    public PathFinder pathFinder;
    void Start()
    {
        linePath = pathFinder.path;
        lineRenderer = GetComponent<LineRenderer>();
        pathFound = pathFinder.pathFound;
    }

    void Update()
    {
        if (pathFound)
        {
            lineRenderer.positionCount = linePath.Length;

            for (int i = 0; i < linePath.Length; i++)
            {
                lineRenderer.SetPosition(i, linePath[i].transform.position);
            }
        }
    }
}
