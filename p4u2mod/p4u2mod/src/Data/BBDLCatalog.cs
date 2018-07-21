using System.IO;

namespace P4U2Mod
{
    public class BBDLCatalog
    {
        // size of "catalog_env.info" file
        public uint size = 0;

        // checksum of "catalog_env.info" file
        public uint checksum = 0;

        public uint SizeOf { get { return 0x10; } }

        public BBDLCatalog Read(BBDLReader r)
        {
            size = r.ReadUInt32();
            checksum = r.ReadUInt32();

            r.BaseStream.Seek(0x8, SeekOrigin.Current);

            return this;
        }

        public void Write(BBDLWriter w)
        {
            w.Write(size);
            w.Write(checksum);

            w.Seek(0x8, SeekOrigin.Current);
        }

        public uint Checksum()
        {
            return size + checksum;
        }
    }
}
