namespace Woomiibo
{
    public static class Utils
    {
        public static TemporarySeekHandle TemporarySeek(this Stream stream, long offset, SeekOrigin origin)
        {
            var ret = stream.Position;
            stream.Seek(offset, origin);
            return new TemporarySeekHandle(stream, ret);
        }
    }
}
