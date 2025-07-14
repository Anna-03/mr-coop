using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FXManager : MonoBehaviour
{
    public GameLogicManager gameLogicManager;
    public GameObject pillarsObj;
    GameObject[] pillars;
    GameObject[,] stations = new GameObject[3, 5];
    GameObject[,] sockets = new GameObject[5, 5];
    public RobotManager[] robots;
    RobotManager currentRobot;

    public LineManager lineStart;
    public LineManager line1;
    public LineManager line2;


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

    public void disconnectFullWire(int disconnectedRow, int disconnectedColumn, int otherRow, int otherColumn, int id)
    {
        // Debug.Log("Wire has been disconnected at: " + disconnectedRow + " , " + disconnectedColumn);
        Vector3 endPosition = sockets[otherRow, otherColumn].transform.position;
        Vector3 startPosition = sockets[disconnectedRow, disconnectedColumn].transform.position;
        if (id < 3) // wire 1
        {
            line1.DisconnectLine(startPosition, endPosition);
        }
        else // wire 2
        {
            line2.DisconnectLine(startPosition, endPosition);
        }
    }

    public void connectFullWire(int connectedRow, int connectedColumn, int otherRow, int otherColumn, int id)
    {

        // Debug.Log("Wire has been connected to: " + connectedRow + " , " + connectedColumn);
        Vector3 endPosition = sockets[otherRow, otherColumn].transform.position;
        Vector3 startPosition = sockets[connectedRow, connectedColumn].transform.position;
        if (id < 3) // wire 1
        {
            line1.ConnectLine(startPosition, endPosition);
        }
        else // wire 2
        {
            line2.ConnectLine(startPosition, endPosition);
        }
    }

    public void disconnectStartWire(int disconnectedColumn)
    {
        Vector3 startWireEndPosition = pillarsObj.transform.position + new Vector3(0f, 2.2f, 0f);
        Vector3 startWireStartPosition = sockets[0, disconnectedColumn].transform.position;
        lineStart.DisconnectLine(startWireStartPosition, startWireEndPosition);
    }

    public void connectStartWire(int connectedColumn)
    {
        Vector3 startWireEndPosition = pillarsObj.transform.position + new Vector3(0f, 2.2f, 0f);
        Vector3 startWireStartPosition = sockets[0, connectedColumn].transform.position;
        lineStart.ConnectLine(startWireStartPosition, startWireEndPosition);
    }

    // public IEnumerator runBuildSequence()
    // {

    //     switch (gameLogicManager.robotCount)
    //     {
    //         case 0:
    //             currentRobot = robots[0];
    //             break;
    //         case 1:
    //             currentRobot = robots[1];
    //             break;
    //         case 2:
    //             currentRobot = robots[2];
    //             break;
    //         default:
    //             currentRobot = robots[0];
    //             break;
    //     }
    //     yield return currentRobot.MoveTo();
    // }


}
