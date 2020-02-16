public enum ClientPackets
{
    ClientHelloServer = 1,
    ClientName,
}

public static class DataSender
{
    public static void SendHelloServer()
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write((int) ClientPackets.ClientHelloServer);
        buffer.Write("Thank I am now connected to you!");
        ClientTcp.SendData(buffer.ToArray());
        buffer.Dispose();
    }

    public static void SentClientName(string name)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write((int) ClientPackets.ClientName);
        buffer.Write(name);
        ClientTcp.SendData(buffer.ToArray());
        buffer.Dispose();
    }
}