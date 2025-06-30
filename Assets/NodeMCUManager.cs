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
public class NodeMCUManager : MonoBehaviour
{
    UdpClient udpClient;
    IPEndPoint remoteEndPoint;
    int localPort = 4211;
    int sendPort = 4210;
    string nodeMCUIP = "192.168.189.56"; // IP of NodeMCU

    int[,] statesArray = new int[5, 5];
    int numMessages;
    private ConcurrentQueue<int[]> receivedStatesQueue = new ConcurrentQueue<int[]>();


    void Start()
    {
        for (int row = 0; row < statesArray.GetLength(0); row++)     // 0 = number of rows
        {
            string rowValues = "";
            for (int col = 0; col < statesArray.GetLength(1); col++) // 1 = number of columns
            {
                statesArray[row, col] = -1;
                rowValues += statesArray[row, col] + " ";
                // Debug.Log($"statesArray[{row},{col}] = {value}");
            }
        }
        // Call the PrintMessage method every 1 second, starting after 1 second
        udpClient = new UdpClient(localPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(nodeMCUIP), sendPort);
        udpClient.BeginReceive(ReceiveCallback, null);

        InvokeRepeating("SendUDPMessage", 1f, 2f);
        // Repeat(() => SendUDPMessage("Hello from Unity!"), 1f, 2f);
        Debug.Log("Started");
    }
    void printStates()
    {
        for (int row = 0; row < statesArray.GetLength(0); row++)     // 0 = number of rows
        {
            string rowValues = "";
            for (int col = 0; col < statesArray.GetLength(1); col++) // 1 = number of columns
            {
                statesArray[row, col] = -1;
                rowValues += statesArray[row, col] + " ";
                // Debug.Log($"statesArray[{row},{col}] = {value}");
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
    // void SendUDPMessage(string message)
    // {
    //     byte[] data = Encoding.UTF8.GetBytes(message);
    //     udpClient.Send(data, data.Length, remoteEndPoint);
    //     Debug.Log("Message sent");
    // }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
        byte[] data = udpClient.EndReceive(ar, ref anyIP);
        // string text = Encoding.UTF8.GetString(data);
        int[] states = BytesToIntArray(data);
        // Debug.Log("Received: " + text);
        // Debug.Log("Received states: " + string.Join(", ", states));
        receivedStatesQueue.Enqueue(states);
        numMessages++;
        // Restart listening
        udpClient.BeginReceive(ReceiveCallback, null);
    }
    void updateStatesArray()
    {

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
        udpClient.Close();
    }
    // Update is called once per frame
    void Update()
    {
        while (receivedStatesQueue.TryDequeue(out int[] states))
        {
            Debug.Log("Received states: " + string.Join(", ", states));
            Debug.Log(numMessages);
            // Update your statesArray here or do other UI updates
        }
    }

    // public void Repeat(Action action, float initialDelay, float repeatRate)
    // {
    //     StartCoroutine(RepeatCoroutine(action, initialDelay, repeatRate));
    // }

    // private IEnumerator RepeatCoroutine(Action action, float initialDelay, float repeatRate)
    // {
    //     yield return new WaitForSeconds(initialDelay);

    //     while (true)
    //     {
    //         action?.Invoke();
    //         yield return new WaitForSeconds(repeatRate);
    //     }
    // }
}
