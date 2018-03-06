using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class Rabbit : MonoBehaviour
{
    public Sockets mySockets; 

    public void ReceiveQuestion(int connID, byte[] recBuffer)
    {
        Question receiveQuestion = (Question)ByteArrayToObject(recBuffer);

        Debug.Log(receiveQuestion.mainQuestion);

        string response = "No, candy is bad for your teeth.";

        byte error;
        byte[] sendBuffer = ObjectToByteArray(response);
        NetworkTransport.Send(mySockets.hostId, connID, 2, sendBuffer, sendBuffer.Length, out error); 
    }

    public static byte[] ObjectToByteArray(object ob)
    {
        MemoryStream ms = new MemoryStream();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(ms, ob);
        return ms.ToArray();
    }
    public static object ByteArrayToObject(byte[] b)
    {
        MemoryStream ms = new MemoryStream(b);
        BinaryFormatter bf = new BinaryFormatter();
        ms.Position = 0;
        return bf.Deserialize(ms);
    }
}

[System.Serializable]              
public class Question
{
    public string mainQuestion;
    public string secondQuestion;
}