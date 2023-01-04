using AnnoRDA.IO.Encryption;
using AnnoRDA.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnnoRDA.IO.Compression;

namespace AnnoRDA
{
    public class BlockContentsSource
    {
        public string ArchiveFilePath { get; }

        public struct BlockFlags
        {
            public int Value { get; }

            public bool IsCompressed
            {
                get
                {
                    return (this.Value & 0x1) != 0;
                }
            }
            public bool IsEncrypted
            {
                get
                {
                    return (this.Value & 0x2) != 0;
                }
            }
            public bool IsMemoryResident
            {
                get
                {
                    return (this.Value & 0x4) != 0;
                }
            }
            public bool IsDeleted
            {
                get
                {
                    return (this.Value & 0x8) != 0;
                }
            }

            public BlockFlags(int value)
            {
                this.Value = value;
            }
            public BlockFlags(bool isCompressed, bool isEncrypted, bool isMemoryResident, bool isDeleted)
            {
                this.Value = (isCompressed ? 0x1 : 0) | (isEncrypted ? 0x2 : 0) | (isMemoryResident ? 0x4 : 0) | (isDeleted ? 0x8 : 0);
            }
        }
        public BlockFlags Flags { get; }

        public long Position { get; }

        public long CompressedSize { get; }
        public long UncompressedSize { get; }

        public BlockContentsSource(string archiveFilePath, BlockFlags flags, long position, long compressedSize, long uncompressedSize)
        {
            if (archiveFilePath == null)
            {
                throw new ArgumentNullException("archiveFilePath");
            }
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException("position cannot be negative.", "position");
            }
            if (compressedSize < 0)
            {
                throw new ArgumentOutOfRangeException("compressedSize cannot be negative.", "compressedSize");
            }
            if (uncompressedSize < 0)
            {
                throw new ArgumentOutOfRangeException("uncompressedSize cannot be negative.", "uncompressedSize");
            }

            this.ArchiveFilePath = archiveFilePath;
            this.Flags = flags;
            this.Position = position;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;
        }
        public BlockContentsSource(string archiveFilePath, BlockFlags flags)
        {
            if (archiveFilePath == null)
            {
                throw new ArgumentNullException("archiveFilePath");
            }
            if (flags.IsMemoryResident)
            {
                throw new ArgumentOutOfRangeException("must provide position, compressedSize and uncompressedSize when memory resident", "flags.IsMemoryResident");
            }

            this.ArchiveFilePath = archiveFilePath;
            this.Flags = flags;
            this.Position = 0;
            this.CompressedSize = 0;
            this.UncompressedSize = 0;
        }

        public Stream GetRawReadStream()
        {
            Stream stream = null;
            try
            {
                stream = new FileStream(this.ArchiveFilePath, FileMode.Open, FileAccess.Read);
                if (this.Flags.IsMemoryResident)
                {
                    stream = new SubStream(stream, this.Position, this.CompressedSize);
                    using (var tempReader = new BinaryReader(stream))
                    {
                        stream = new MemoryStream(tempReader.ReadBytes((int)this.UncompressedSize));
                    }
                }
                return stream;
            }
            catch
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                throw;
            }
        }
        public Stream GetReadStream()
        {
            Stream stream = null;
            try
            {
                stream = new FileStream(this.ArchiveFilePath, FileMode.Open, FileAccess.Read);
                if (this.Flags.IsMemoryResident)
                {
                    stream = new SubStream(stream, this.Position, this.CompressedSize);
                    if (this.Flags.IsEncrypted)
                    {
                        stream = new EncryptionStream(stream, StreamAccessMode.Read);
                    }
                    if (this.Flags.IsCompressed)
                    {
                        stream = new ZlibStream(stream, System.IO.Compression.CompressionMode.Decompress);
                    }
                    using (var tempReader = new BinaryReader(stream))
                    {
                        stream = new MemoryStream(tempReader.ReadBytes((int)this.UncompressedSize));
                    }
                }
                return stream;
            }
            catch
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
                throw;
            }
        }

        public BlockContentsSource DeepClone()
        {
            return new BlockContentsSource(
                this.ArchiveFilePath,
                this.Flags,
                this.Position,
                this.CompressedSize,
                this.UncompressedSize
            );
        }
    }
}
