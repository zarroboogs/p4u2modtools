using System.IO;
using System.Text;
using System.Linq;
using System;

namespace P4U2Mod
{
    public class BBDLFileListing
    {
        // size of listed file
        public uint size = 0;

        // checksum of listed file contents
        public uint checksum = 0;

        // path of listed file, relative to "data\"
        public string path = string.Empty;

        public uint SizeOf { get { return 0x90; } }

        public BBDLFileListing Read(BBDLReader r)
        {
            size = r.ReadUInt32();
            checksum = r.ReadUInt32();

            r.BaseStream.Seek(0x8, SeekOrigin.Current);

            path = new string(r.ReadChars(0x80));

            return this;
        }

        public void Write(BBDLWriter w)
        {
            w.Write(size);
            w.Write(checksum);

            w.Seek(0x8, SeekOrigin.Current);

            w.Write(path.PadRight(0x80, '\0').ToCharArray());
        }

        public uint Checksum()
        {
            var s = size + checksum;
            var b = new byte[0x80];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(path), 0, b, 0, path.Length);
            return s + Util.ChecksumUInt32BE(b);
        }
    }
}
