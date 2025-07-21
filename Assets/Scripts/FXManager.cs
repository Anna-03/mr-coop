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

    Material lightGreen;
    Material lightRed;
    Material lightOff;
    Material lightOn;

    Animator animator;
    Transform stationObject;

    [SerializeField]
    float startHeight = 2.2f;

    void Start()
    {
        // don't use in Start! Only in Update!
        pillars = gameLogicManager.pillars;
        stations = gameLogicManager.stations;
        sockets = gameLogicManager.sockets;

        lightRed = Resources.Load<Material>("Materials/Stations/M_LampFalse");
        lightGreen = Resources.Load<Material>("Materials/Stations/M_LampTrue");
        lightOff = Resources.Load<Material>("Materials/Stations/M_LampOFF");
        lightOn = Resources.Load<Material>("Materials/Stations/M_LampConnected");

    }

    void Update()
    {

    }
    public void changeSocketState(int row, int column, int state)
    {
        GameObject socket = sockets[row, column];
        Renderer renderer = socket.GetComponent<Renderer>();
        Color color = Color.gray;
        Material lampMat = lightOff;
        // state: 0 = off, 1 = connected, 2 = correct connection, 3 = wrong connection
        switch (state)
        {
            case 0:
                color = Color.gray;
                lampMat = lightOff;
                break;
            case 1:
                color = Color.white;
                lampMat = lightOn;
                break;
            case 2:
                color = Color.green;
                lampMat = lightGreen;
                break;
            case 3:
                color = Color.red;
                lampMat = lightRed;
                break;
            default:
                break;
        }
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.color = color;
            stationObject = socket.transform.parent.Find("visual/Station");
            if (stationObject != null){
              Renderer rend = stationObject.GetComponent<Renderer>();
              if (rend != null){
                // 0 is main body, 1 is display, 2 is lamps
                rend.materials[0] = lampMat;
                rend.materials[1] = lampMat;
                rend.materials[2] = lampMat;
              } else{Debug.LogWarning("Renderer not found on stationObject.");}
            } else{Debug.LogWarning("Could not find child path: visual/Station");}
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
        Vector3 startWireEndPosition = pillarsObj.transform.position + new Vector3(0f, startHeight, 0f);
        Vector3 startWireStartPosition = sockets[0, disconnectedColumn].transform.position;
        lineStart.DisconnectLine(startWireStartPosition, startWireEndPosition);
    }

    public void connectStartWire(int connectedColumn)
    {
        Vector3 startWireEndPosition = pillarsObj.transform.position + new Vector3(0f, startHeight, 0f);
        Vector3 startWireStartPosition = sockets[0, connectedColumn].transform.position;
        lineStart.ConnectLine(startWireStartPosition, startWireEndPosition);
    }

    public IEnumerator runBuildSequence()
    {

        switch (gameLogicManager.robotCount)
        {
            case 0:
                currentRobot = robots[2];
                break;
            case 1:
                currentRobot = robots[0];
                break;
            // case 2:
            //     currentRobot = robots[2];
            //     break;
            default:
                currentRobot = robots[0];
                break;
        }

        GameObject[] connectedSockets = new GameObject[]{
            sockets[0, gameLogicManager.wireStart.connectedColumn],                                // id =0
            sockets[gameLogicManager.wire1.connection1[0], gameLogicManager.wire1.connection1[1]], // id =1
            sockets[gameLogicManager.wire1.connection2[0], gameLogicManager.wire1.connection2[1]], // id =2
            sockets[gameLogicManager.wire2.connection1[0], gameLogicManager.wire2.connection1[1]], // id =3
            sockets[gameLogicManager.wire2.connection2[0], gameLogicManager.wire2.connection2[1]], // id =4
        };

        Array.Sort(connectedSockets, (a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

        int row3Column = 0;
        int row4Column = 0;
        for (int column = 0; column < 5; column++)
        {
            if (sockets[3, column] == connectedSockets[3])
            {
                row3Column = column;
            }
            if (sockets[4, column] == connectedSockets[4])
            {
                row4Column = column;
            }
        }



        currentRobot.ingotMesh.enabled = true;
        // moving from middle top to first station input
        yield return currentRobot.MoveTo(pillarsObj.transform.position + new Vector3(0f, startHeight, 0f), connectedSockets[0].transform.position);
        GameObject visual = connectedSockets[0].transform.Find("visual");
        Renderer station = visual.transform.Find("Station").GetComponent<Renderer>();
        station.materials[0].color = Color.blue;
        // wait for Station Animation
        currentRobot.ingotMesh.enabled = false;
        currentRobot.bodyMesh.enabled = true;
        // moving from first station ouput to second station input
        yield return currentRobot.MoveTo(connectedSockets[1].transform.position, connectedSockets[2].transform.position);
        // wait for Station Animation
        currentRobot.bodyMesh.material = currentRobot.materialLookup[row3Column];
        // moving from second station output to third station input
        yield return currentRobot.MoveTo(connectedSockets[3].transform.position, connectedSockets[4].transform.position);
        // wait for Station Animation
        currentRobot.itemRendererLookup[row4Column].enabled = true;
        // robot jumps out of third station
        yield return currentRobot.MoveTo(connectedSockets[4].transform.position, connectedSockets[4].transform.position + new Vector3(0f, 0.2f, 0f)); // maybe replace with jump animation


        gameLogicManager.isBuilding = false;
        if (gameLogicManager.robotCount < 1) // TODO: change back to 2 after test
        {
            gameLogicManager.robotCount++;
        }
        else
        {
            gameLogicManager.robotCount = 0;  // TODO: remove this line, only for testing
            gameLogicManager.gameIsFinished = true;
            //trigger end scene
        }

    }
    public void ResetRobots()
    {
        for (int robotId = 0; robotId < robots.Length; robotId++)
        {
            robots[robotId].ingotMesh.enabled = false;
            robots[robotId].bodyMesh.enabled = false;
            robots[robotId].bodyMesh.material = robots[robotId].ingotMaterial;
            for (int itemId = 0; itemId < robots[robotId].itemRendererLookup.Length; itemId++)
            {
                robots[robotId].itemRendererLookup[itemId].enabled = false;
            }
        }
    }


}
