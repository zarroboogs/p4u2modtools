using System;
using System.IO;

namespace P4U2Mod
{
    public class BBDLReader : BinaryReader
    {
        protected byte[] b32 = new byte[4];

        public BBDLReader(Stream stream) : base(stream) { }

        public override uint ReadUInt32()
        {
            b32 = base.ReadBytes(4);
            Array.Reverse(b32);
            return BitConverter.ToUInt32(b32, 0);
        }
    }
}
