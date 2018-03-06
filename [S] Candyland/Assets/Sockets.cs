using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class Sockets : MonoBehaviour
{
    public Rabbit myRabbit;

    public int hostId;           
    public int hostPort;         
    public string hostAddress;  

    public int connectionId;   
     
    public int chan0;        
    public int chan1;        
    public int chan2;

    int bs;            

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
                Debug.Log("Connection | " + recConnectionId);
                break;
            
            case NetworkEventType.DataEvent:
                bs = dataSize;
                if (channelId == 1) { myRabbit.ReceiveQuestion(recConnectionId, recBuffer); }
                //if (channelId == 2) { Receive a different message. Do something else.  }
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("Disconnection || " + recConnectionId);
                break;
        }
    }
}
