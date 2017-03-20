using System;
using System.IO;
using System.Threading;

namespace ClojureRepl
{
    public class BlockingStream : Stream
    {
        private readonly Stream _stream;

        public BlockingStream(Stream stream)
        {
            if (!stream.CanSeek)
                throw new ArgumentException("Stream must support seek", "stream");
            _stream = stream;
        }

        public override void Flush()
        {
            lock (_stream)
            {
                _stream.Flush();
                Monitor.Pulse(_stream);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (_stream)
            {
                long res = _stream.Seek(offset, origin);
                Monitor.Pulse(_stream);
                return res;
            }
        }

        public override void SetLength(long value)
        {
            lock (_stream)
            {
                _stream.SetLength(value);
                Monitor.Pulse(_stream);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (_stream)
            {
                do
                {
                    int read = _stream.Read(buffer, offset, count);
                    if (read > 0)
                        return read;
                    Monitor.Wait(_stream);
                } while (true);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_stream)
            {
                long currentPosition = _stream.Position;
                _stream.Position = _stream.Length;
                _stream.Write(buffer, offset, count);
                _stream.Position = currentPosition;
                Monitor.Pulse(_stream);
            }
        }

        public override bool CanRead
        {
            get
            {
                lock (_stream)
                {
                    return _stream.CanRead;
                }
            }
        }

        public override bool CanSeek
        {
            get
            {
                lock (_stream)
                {
                    return _stream.CanSeek;
                }
            }
        }

        public override bool CanWrite
        {
            get
            {
                lock (_stream)
                {
                    return _stream.CanWrite;
                }
            }
        }

        public override long Length
        {
            get
            {
                lock (_stream)
                {
                    return _stream.Length;
                }
            }
        }

        public override long Position
        {
            get
            {
                lock (_stream)
                {
                    return _stream.Position;
                }
            }
            set
            {
                lock (_stream)
                {
                    _stream.Position = value;
                    Monitor.Pulse(_stream);
                }
            }
        }
    }
}