using System;
using System.Linq;
using System.Text;

namespace P4U2Mod
{
    public class BBDLVersion
    {
        // version index
        public uint index = 0;

        // offset of this version's file listings in the update file
        // i.e. the sum of the total file count of all previous versions
        public uint offset = 0;

        // total file size, in this version only
        public uint ver_file_size = 0;

        // total file count, in this version only
        public uint ver_file_count = 0;

        // version path, relative to the update file's folder
        public string path = string.Empty;

        public uint SizeOf { get { return 0x30; } }

        public BBDLVersion Read(BBDLReader r)
        {
            index = r.ReadUInt32();
            offset = r.ReadUInt32();
            ver_file_size = r.ReadUInt32();
            ver_file_count = r.ReadUInt32();
            path = new string(r.ReadChars(0x20));

            return this;
        }

        public void Write(BBDLWriter w)
        {
            w.Write(index);
            w.Write(offset);
            w.Write(ver_file_size);
            w.Write(ver_file_count);
            w.Write(path.PadRight(0x20, '\0').ToCharArray());
        }

        public uint Checksum()
        {
            var s = index + offset + ver_file_count + ver_file_size;
            var b = new byte[0x20];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(path), 0, b, 0, path.Length);
            return s + Util.ChecksumUInt32BE(b);
        }
    }
}
