using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace P4U2Mod
{
    class Program
    {
        static void Main(string[] args)
        {
            string updDirPath = Directory.GetCurrentDirectory();
            string updFilePath = Path.Combine(updDirPath, "UpdateFileList.bin");

            bool calcFileChecksums = false;

            BBDLFile bbdlFile = new BBDLFile();

            var dirs = Directory.EnumerateDirectories(updDirPath, "ver_*", SearchOption.TopDirectoryOnly);

            foreach (var dir in dirs)
            {
                BBDLVersion v = new BBDLVersion
                {
                    path = dir.Split('\\').Last(),
                    index = (uint)bbdlFile.versions.Count(),
                    offset = (uint)bbdlFile.versions.Sum(x => x.ver_file_count)
                };

                bbdlFile.versions.Add(v);

                var files = Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories);
                Regex rgxPath = new Regex(@"(data\\.*)");

                foreach (var file in files)
                {
                    BBDLFileListing fl = new BBDLFileListing
                    {
                        path = rgxPath.Match(file).Value,
                        size = (uint)new FileInfo(file).Length,
                        checksum = calcFileChecksums ? Util.ChecksumUInt32BE(file) : 0
                    };

                    bbdlFile.file_listings.Add(fl);

                    v.ver_file_size += fl.size;
                    v.ver_file_count++;

                    Console.WriteLine("{0}: {1}", v.path, fl.path);
                }

                bbdlFile.header.total_file_count += v.ver_file_count;
                bbdlFile.header.total_file_size += v.ver_file_size;
                bbdlFile.header.version_count++;
            }

            var catPath = Path.Combine(updDirPath, "catalog_env.info");
            bbdlFile.catalog.checksum = Util.ChecksumUInt32BE(catPath);
            bbdlFile.catalog.size = (uint) new FileInfo(catPath).Length;

            bbdlFile.header.checksum = bbdlFile.Checksum();
            bbdlFile.header.rounded_size = bbdlFile.SizeOf / 0x0f * 0x0f;
            
            Util.WriteBBDL(updFilePath, bbdlFile);
        }
    }
}
