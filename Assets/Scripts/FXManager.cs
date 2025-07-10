using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeSocketState(int row, int column, int state)
    {
        // state: 0 = off, 1 = connected, 2 = wrong connection, 3 = correct connection
    }

    public void disconnectWire(int disconnectedRow, int disconnectedColumn, int otherRow, int otherColumn)
    {
        
    }

    public void connectWire(int connectedRow, int connectedColumn, int otherRow, int otherColumn)
    {

    }
    
    public void disconnectStartWire(int disconnectedColumn)
    {
        
    }

    public void connectStartWire(int connectedColumn)
    {

    }
}
