using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class Sockets : MonoBehaviour
{
    public Squirrel mySquirrel;

    int bs;                 
    public int hostId;
    public int hostPort;
    public string hostAddress;

    public int connectionId;

    public int chan0;
    public int chan1;
    public int chan2;
 
    void Initialize()
    {
        hostPort = 8899;
        hostAddress = "your.ip.address.here";

        ConnectionConfig config = new ConnectionConfig();
        chan0 = config.AddChannel(QosType.Reliable);
        chan1 = config.AddChannel(QosType.ReliableFragmented);
        chan2 = config.AddChannel(QosType.ReliableFragmented);

        HostTopology topology = new HostTopology(config, 10);

        hostId = NetworkTransport.AddHost(topology, hostPort);
    }

    public void Connect()
    {
        byte error;
        connectionId = NetworkTransport.Connect(hostId, hostAddress, hostPort, 0, out error);
    }

    public void Disconnect()
    {
        byte error;
        NetworkTransport.Disconnect(hostId, connectionId, out error);
    }

    void Awake()
    {
        NetworkTransport.Init();
        Initialize();
    }

    void Update()
    {
        int recHostId;
        int recConnectionId;
        int channelId;
        byte[] recBuffer = new byte[bs];
        int bufferSize = bs;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out recConnectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log("Connected to host");
                break;

            case NetworkEventType.DataEvent:
                bs = dataSize;

                if (channelId == 2) { mySquirrel.ReceiveResponse(recBuffer); }

                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnected from host");
                break;
        }
    }
}

