using System;
using Xunit.Abstractions;

namespace DBMegreat.MigrationTools.Test.Utils
{
    public class TestLogger : BaseLogger, ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly ILogger _decorated;

        public TestLogger(ITestOutputHelper output)
        {
            _output = output;
            _decorated = new EmptyLogger();
        }

        public TestLogger(ITestOutputHelper output, ILogger decorated)
        {
            _output = output;
            _decorated = decorated;
        }

        public void Error(string message, Exception ex)
        {
            _decorated.Error(message, ex);
            _output.WriteLine(FormatError(message, ex));
        }

        public void Info(string message)
        {
            _decorated.Info(message);
            _output.WriteLine(FormatInfo(message));
        }

        public void Warning(string message)
        {
            _decorated.Warning(message);
            _output.WriteLine(FormatWarning(message));
        }
    }
}
