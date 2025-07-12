using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FXManager : MonoBehaviour
{
    public GameLogicManager gameLogicManager;
    GameObject[] pillars;
    GameObject[,] stations = new GameObject[3, 5];
    GameObject[,] sockets = new GameObject[5, 5];

    void Start()
    {
        // don't use in Start! Only in Update!
        pillars = gameLogicManager.pillars;
        stations = gameLogicManager.stations;
        sockets = gameLogicManager.sockets;

    }

    void Update()
    {

    }
    public void changeSocketState(int row, int column, int state)
    {
        GameObject socket = sockets[row, column];
        Renderer renderer = socket.GetComponent<Renderer>();
        Color color = Color.gray;
        // state: 0 = off, 1 = connected, 2 = correct connection, 3 = wrong connection
        switch (state)
        {
            case 0:
                color = Color.gray;
                break;
            case 1:
                color = Color.white;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.red;
                break;
            default:
                break;
        }
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.color = color;
        }
    }

    public void disconnectFullWire(int disconnectedRow, int disconnectedColumn, int otherRow, int otherColumn)
    {
        // Debug.Log("Wire has been disconnected at: " + disconnectedRow + " , " + disconnectedColumn);
    }

    public void connectFullWire(int connectedRow, int connectedColumn, int otherRow, int otherColumn)
    {
        // Debug.Log("Wire has been connected to: " + connectedRow + " , " + connectedColumn);
    }

    public void disconnectStartWire(int disconnectedColumn)
    {

    }

    public void connectStartWire(int connectedColumn)
    {

    }

}
