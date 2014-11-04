﻿namespace KellyElton.Networking
{
    public interface ISocketMessageProcessor
    {
        void AddData(byte[] data);

        byte[] PopMessage();

        void Clear();
    }
}