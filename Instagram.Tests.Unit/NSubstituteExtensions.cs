using NSubstitute;

namespace Instagram.Tests.Unit
{
    public static class NSubstituteExtensions
    {
        public static int GetMethodCallsNumber<T>(this T mock, string methodName) where T : class
            => mock.ReceivedCalls().Count(c => c.GetMethodInfo().Name == methodName);
    }
}
