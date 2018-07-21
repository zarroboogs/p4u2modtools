using System;
using System.IO;

namespace P4U2Mod
{
    public class BBDLWriter : BinaryWriter
    {
        protected byte[] b32 = new byte[4];

        public BBDLWriter(Stream stream) : base(stream) { }

        public override void Write(uint value)
        {
            b32 = BitConverter.GetBytes(value);
            Array.Reverse(b32);
            base.Write(b32);
        }
    }
}
