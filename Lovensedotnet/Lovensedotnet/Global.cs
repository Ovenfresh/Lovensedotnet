using System.Threading;

namespace Lovensedotnet
{
    public static class Global
    {
        public static CancellationTokenSource PingCancellationTokens = new();
    }
}
