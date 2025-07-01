using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Meta.XR.ImmersiveDebugger;
using Oculus.Interaction;
using UnityEngine.UIElements;

public class NodeMCUManagerThread : MonoBehaviour
{
    UdpClient udpClient;
    IPEndPoint remoteEndPoint;
    Thread receiveThread;
    bool running = true;

    int localPort = 4211;
    int sendPort = 4210;
    string nodeMCUIP = "192.168.189.56"; // IP of NodeMCU

    public int[,] statesArray = new int[5, 5];
    int numMessages;
    private ConcurrentQueue<int[]> receivedStatesQueue = new ConcurrentQueue<int[]>();

    void Start()
    {
        for (int row = 0; row < statesArray.GetLength(0); row++)
        {
            for (int col = 0; col < statesArray.GetLength(1); col++)
            {
                statesArray[row, col] = -1;
            }
        }

        udpClient = new UdpClient(localPort);
        udpClient.Client.ReceiveBufferSize = 8192;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(nodeMCUIP), sendPort);

        receiveThread = new Thread(ReceiveLoop);
        receiveThread.IsBackground = true;
        receiveThread.Start();

        InvokeRepeating("SendUDPMessage", 1f, 2f);
        Debug.Log("Started (threaded UDP)");
    }
    void Update()
    {
        while (receivedStatesQueue.TryDequeue(out int[] states))
        {
            UpdateStatesArray(states);
            PrintStatesArray();
            // Debug.Log("Received states: " + string.Join(", ", states));
            // Debug.Log($"Total messages received: {numMessages}");
            // Update your statesArray here or do other UI updates
        }
    }

    void ReceiveLoop()
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

        while (running)
        {
            try
            {
                byte[] data = udpClient.Receive(ref anyIP);
                int[] states = BytesToIntArray(data);
                receivedStatesQueue.Enqueue(states);
                Interlocked.Increment(ref numMessages);
            }
            catch (SocketException ex)
            {
                if (running)
                    Debug.LogWarning("Socket exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                Debug.LogError("Receive thread error: " + ex.Message);
            }
        }
    }

    void SendUDPMessage()
    {
        string message = "Hello from Unity!";
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, remoteEndPoint);
        Debug.Log("Message sent");
    }

    void UpdateStatesArray(int[] states)
    {
        if (states.Length == 4)
        {
            int column = states[0] / 2;
            statesArray[0, column] = states[2];
            statesArray[1, column] = states[3];
        }
        else if (states.Length == 5)
        {
            int column = states[0] / 2;
            statesArray[2, column] = states[2];
            statesArray[3, column] = states[3];
            statesArray[4, column] = states[4];
        }
        Debug.Log("messages count: " + states[1]);

    }

    void PrintStatesArray()
    {
        int rowsLength = statesArray.GetLength(0);
        int columnsLength = statesArray.GetLength(1);

        for (int row = 0; row < rowsLength; row++)
        {
            string[] rowValues = new string[columnsLength];
            for (int col = 0; col < columnsLength; col++)
            {
                rowValues[col] = statesArray[row, col].ToString();
            }

            Debug.Log(string.Join(", ", rowValues));
        }

        Debug.Log("");
    }

    int[] BytesToIntArray(byte[] bytes)
    {
        int intCount = bytes.Length / 4;
        int[] result = new int[intCount];
        for (int i = 0; i < intCount; i++)
        {
            result[i] = BitConverter.ToInt32(bytes, i * 4);
        }
        return result;
    }


    void OnApplicationQuit()
    {
        running = false;
        udpClient?.Close();           // This unblocks Receive()
        receiveThread?.Join();        // Wait for thread to exit cleanly
    }
}