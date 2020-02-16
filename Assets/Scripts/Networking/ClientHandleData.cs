using System.Collections.Generic;

public static class ClientHandleData
{
    private static ByteBuffer playerBuffer;
    public delegate void Packet(byte[] data);
    public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

    public static void InitializePackets()
    {
        packets.Add((int) ServerPackets.ServerWelcomeMessage, DataReceiver.HandleWelcomeMessage);
        packets.Add((int) ServerPackets.WelcomeClientWithName, DataReceiver.HandleWelcomeClientWithName);
    }

    public static void HandleData(byte[] data)
    {
        byte[] buffer = (byte[]) data.Clone();
        int packetLength = 0;

        if (playerBuffer == null)
        {
            playerBuffer = new ByteBuffer();
        }

        playerBuffer.Write(buffer);

        if (playerBuffer.Count == 0)
        {
            playerBuffer.Clear();
            return;
        }

        if (playerBuffer.Length >= 4)
        {
            packetLength = playerBuffer.ReadInt(false);
            if (packetLength <= 0)
            {
                playerBuffer.Clear();
                return;
            }
        }

        while (packetLength > 0 && packetLength <= playerBuffer.Length - 4)
        {
            if (packetLength <= playerBuffer.Length - 4)
            {
                playerBuffer.ReadInt();
                data = playerBuffer.ReadBytes(packetLength);
                HandleDataPackets(data);
            }

            packetLength = 0;

            if (playerBuffer.Length >= 4)
            {
                packetLength = playerBuffer.ReadInt(false);
                if (packetLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }
        }

        if (packetLength <= 1)
        {
            playerBuffer.Clear();
        }
    }

    private static void HandleDataPackets(byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer();
        buffer.Write(data);
        int packedId = buffer.ReadInt();
        buffer.Dispose();

        if (packets.TryGetValue(packedId, out Packet packet))
        {
            packet.Invoke(data);
        }
    }
}