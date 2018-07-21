using System.IO;

namespace P4U2Mod
{
    public static class Util
    {
        public static uint ChecksumUInt32BE(byte[] buffer, uint start = 0)
        {
            uint checksum = 0;
            long l = buffer.Length / sizeof(uint) * sizeof(uint);

            for (uint i = start; i < l; i += 4)
                checksum += (uint)(buffer[i] << 24 | buffer[i + 1] << 16 | buffer[i + 2] << 8 | buffer[i + 3]);

            return checksum;
        }

        public static uint ChecksumUInt32BE(string path, uint start = 0)
        {
            byte[] buffer = File.ReadAllBytes(path);
            return ChecksumUInt32BE(buffer, start);
        }

        public static BBDLFile ReadBBDL(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BBDLReader(stream))
                return new BBDLFile().Read(reader);
        }

        public static void WriteBBDL(string path, BBDLFile bbdl)
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new BBDLWriter(stream))
                bbdl.Write(writer);
        }
    }
}
