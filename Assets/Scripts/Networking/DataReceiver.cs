using UnityEngine;

public enum ServerPackets
{
    ServerWelcomeMessage = 1,
    WelcomeClientWithName,
}

public static class DataReceiver
{
    public static void HandleWelcomeMessage(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write(data);
        int packedId = buffer.ReadInt();
        string message = buffer.ReadString();
        buffer.Dispose();

        Debug.Log(message);
        DataSender.SendHelloServer();
    }
    
    public static void HandleWelcomeClientWithName(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write(data);
        int packedId = buffer.ReadInt();
        string welcomeMessageWithName = buffer.ReadString();
        buffer.Dispose();

        Debug.Log(welcomeMessageWithName);
    }
}