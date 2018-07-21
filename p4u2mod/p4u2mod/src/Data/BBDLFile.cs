using System.Collections.Generic;
using System.Linq;

namespace P4U2Mod
{
    public class BBDLFile
    {
        // file header, not included in checksum
        public BBDLHeader header = new BBDLHeader();

        // "catalog_env.info" details, included in checksum
        public BBDLCatalog catalog = new BBDLCatalog();

        // version list, included in checksum
        public List<BBDLVersion> versions = new List<BBDLVersion>();

        // file list, included in checksum
        public List<BBDLFileListing> file_listings = new List<BBDLFileListing>();

        public uint SizeOf {
            get
            {
                return header.SizeOf + catalog.SizeOf + 
                    (uint) versions.Sum(x => x.SizeOf) + 
                    (uint) file_listings.Sum(x => x.SizeOf);
            }
        }

        public BBDLFile Read(BBDLReader r)
        {
            header.Read(r);
            catalog.Read(r);

            for (int i = 0; i < header.version_count; i++)
                versions.Add(new BBDLVersion().Read(r));

            for (int i = 0; i < header.total_file_count; i++)
                file_listings.Add(new BBDLFileListing().Read(r));

            return this;
        }

        public void Write(BBDLWriter w)
        {
            header.Write(w);
            catalog.Write(w);

            foreach (var v in versions) v.Write(w);
            foreach (var fl in file_listings) fl.Write(w);
        }

        public uint Checksum()
        {
            return catalog.Checksum() + 
                (uint) versions.Sum(x => x.Checksum()) + 
                (uint) file_listings.Sum(x => x.Checksum());
        }
    }
}
