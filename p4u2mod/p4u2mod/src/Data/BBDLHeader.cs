using System.IO;

namespace P4U2Mod
{
    public class BBDLHeader
    {
        // file type
        public string type = "BBDL";

        // unknown, probably size of header
        private uint unk0 = 0x30;

        // current version index
        public uint version_count = 0;

        // unknown
        private uint unk1 = 0x1000;

        // size of this file, rounded down to nearest multiple of 0x10
        public uint rounded_size = 0;

        // total file count, in all versions
        public uint total_file_count = 0;

        // total file size, in all versions
        public uint total_file_size = 0;

        // unknown
        private uint unk2 = 0x03;

        // checksum of this file (without the header)
        public uint checksum = 0;

        public uint SizeOf { get { return 0x30; } }

        public BBDLHeader Read(BBDLReader r)
        {
            type = new string(r.ReadChars(0x4));
            unk0 = r.ReadUInt32();

            r.BaseStream.Seek(0x04, SeekOrigin.Current);

            version_count = r.ReadUInt32();
            unk1 = r.ReadUInt32();
            rounded_size = r.ReadUInt32();
            total_file_count = r.ReadUInt32();
            total_file_size = r.ReadUInt32();
            unk2 = r.ReadUInt32();
            checksum = r.ReadUInt32();

            r.BaseStream.Seek(0x08, SeekOrigin.Begin);

            return this;
        }

        public void Write(BBDLWriter w)
        {
            w.Write(type.ToCharArray());
            w.Write(unk0);

            w.Seek(0x04, SeekOrigin.Current);

            w.Write(version_count);
            w.Write(unk1);
            w.Write(rounded_size);
            w.Write(total_file_count);
            w.Write(total_file_size);
            w.Write(unk2);
            w.Write(checksum);

            w.Seek(0x08, SeekOrigin.Current);
        }
    }
}
