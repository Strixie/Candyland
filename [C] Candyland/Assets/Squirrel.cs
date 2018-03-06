using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class Squirrel : MonoBehaviour {

    public Sockets mySockets; 

    void Start()
    {
        StartCoroutine(DoYouLikeCandy()); 
    }

    public IEnumerator DoYouLikeCandy()
    {
        mySockets.Connect();

        yield return new WaitForSeconds(0.2f);

        Question askQuestion = new Question();
        askQuestion.mainQuestion = "Do you like candy Mr. Server?";
        askQuestion.secondQuestion = "What is your favorite kind of candy?";

        byte error;
        byte[] sendBuffer = ObjectToByteArray(askQuestion);
        NetworkTransport.Send(mySockets.hostId, mySockets.connectionId, 1, sendBuffer, sendBuffer.Length, out error); 
    }

    public void ReceiveResponse(byte[] recBuffer)
    {
        string recResponse = (string)ByteArrayToObject(recBuffer); 

        if(recResponse != "yes")
        {
            Debug.Log("How could you...not like...candy :(   "); 
        }
        else
        {
            Debug.Log("Cool stuff."); 
        }
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
