using System.Threading;

namespace LovenseApiUI
{
    public static class Global
    {
        public static CancellationTokenSource PingCancellationTokens = new();
    }
}
