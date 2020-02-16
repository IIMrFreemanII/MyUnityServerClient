using System;
using System.Net.Sockets;

public static class ClientTcp
{
    private static TcpClient clientSocket;
    private static NetworkStream stream;
    private static byte[] receiveBuffer;
    public static string clientName;

    public static void InitializingNetworking(string host, int port)
    {
        clientSocket = new TcpClient();
        clientSocket.ReceiveBufferSize = 4096;
        clientSocket.SendBufferSize = 4096;
        receiveBuffer = new byte[4096 * 2];

        clientSocket.BeginConnect(host, port, OnClientConnect, clientSocket);
    }

    private static void OnClientConnect(IAsyncResult result)
    {
        clientSocket.EndConnect(result);
        if (clientSocket.Connected == false)
        {
            return;
        }

        clientSocket.NoDelay = true;
        stream = clientSocket.GetStream();
        stream.BeginRead(receiveBuffer, 0, 4096 * 2, ReceiveCallback, null);
    }

    private static void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int length = stream.EndRead(result);
            
            if (length <= 0) return;
            
            byte[] newBytes = new byte[length];
            Array.Copy(receiveBuffer, newBytes, length);

            UnityThread.executeInFixedUpdate(
                () =>
                {
                    ClientHandleData.HandleData(newBytes);
                });
            
            stream.BeginRead(receiveBuffer, 0, 4096 * 2, ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static void SendData(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
        buffer.Write(data);
        stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        buffer.Dispose();
    }

    public static void Disconnect()
    {
        clientSocket.Close();
    }
}
