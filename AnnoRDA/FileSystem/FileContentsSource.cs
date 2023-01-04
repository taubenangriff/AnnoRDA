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
    public class FileContentsSource
    {
        public BlockContentsSource BlockContentsSource { get; }

        public long PositionInBlock { get; }

        public long GlobalPosition
        {
            get
            {
                if (this.BlockContentsSource.Flags.IsMemoryResident && (this.BlockContentsSource.Flags.IsCompressed || this.BlockContentsSource.Flags.IsEncrypted))
                {
                    throw new NotSupportedException("GlobalPosition on files in memory-resident blocks which are also compressed or encrypted is not meaningful");
                }
                return this.BlockContentsSource.Position + this.PositionInBlock;
            }
        }

        public long CompressedSize { get; }
        public long UncompressedSize { get; }

        public FileContentsSource(BlockContentsSource blockContentsSource, long positionInBlock, long compressedSize, long uncompressedSize)
        {
            if (blockContentsSource == null)
            {
                throw new ArgumentNullException("blockContentsSource");
            }
            if (positionInBlock < 0)
            {
                throw new ArgumentOutOfRangeException("positionInBlock cannot be negative.", "positionInBlock");
            }
            if (compressedSize < 0)
            {
                throw new ArgumentOutOfRangeException("compressedSize cannot be negative.", "compressedSize");
            }
            if (uncompressedSize < 0)
            {
                throw new ArgumentOutOfRangeException("uncompressedSize cannot be negative.", "uncompressedSize");
            }

            this.BlockContentsSource = blockContentsSource;
            this.PositionInBlock = positionInBlock;
            this.CompressedSize = compressedSize;
            this.UncompressedSize = uncompressedSize;
        }

        public Stream GetReadStream()
        {
            Stream stream = null;
            try
            {
                stream = this.BlockContentsSource.GetReadStream();
                stream = new SubStream(stream, this.PositionInBlock, this.CompressedSize);
                if (!this.BlockContentsSource.Flags.IsMemoryResident)
                {
                    if (this.BlockContentsSource.Flags.IsEncrypted)
                    {
                        stream = new EncryptionStream(stream, StreamAccessMode.Read);
                    }
                    if (this.BlockContentsSource.Flags.IsCompressed)
                    {
                        stream = new ZlibStream(stream, System.IO.Compression.CompressionMode.Decompress);
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

        public FileContentsSource DeepClone()
        {
            return new FileContentsSource(
                this.BlockContentsSource,
                this.PositionInBlock,
                this.CompressedSize,
                this.UncompressedSize
            );
        }
    }
}
