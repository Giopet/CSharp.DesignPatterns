using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CSharp.DesignPatterns.Singleton.v1_NotThreadSafe
{
    public class SingletonPatternTest
    {
        private readonly ITestOutputHelper _output;

        public SingletonPatternTest(ITestOutputHelper output)
        {
            _output = output;
            SingletonPatternTestHelpers.Reset(typeof(Database));
            Logger.Clear();
        }

        [Fact]
        public void ReturnsNonNullDatabaseInstance()
        {
            Assert.Null(SingletonPatternTestHelpers.GetPrivateStaticInstance<Database>());

            var result = Database.Instance;

            Assert.NotNull(result);
            Assert.IsType<Database>(result);

            Logger.Output().ToList().ForEach(h => _output.WriteLine(h));
        }

        [Fact]
        public void OnlyCallsConstructorOnceGivenThreeInstanceCalls()
        {
            Assert.Null(SingletonPatternTestHelpers.GetPrivateStaticInstance<Database>());

            var one = Database.Instance;
            var two = Database.Instance;
            var three = Database.Instance;

            var log = Logger.Output();
            Assert.Equal(1, log.Count(log => log.Contains("Constructor")));
            Assert.Equal(3, log.Count(log => log.Contains("Instance")));

            Logger.Output().ToList().ForEach(h => _output.WriteLine(h));
        }

        /// <summary>
        /// The simple singleton pattern it is not thread safe.
        /// </summary>
        [Fact]
        public void CallsConstructorMultipleTimesGivenThreeParallelInstanceCalls()
        {
            Assert.Null(SingletonPatternTestHelpers.GetPrivateStaticInstance<Database>());

            // configure logger to slow down the creation long enough to cause problems
            Logger.DelayMilliseconds = 50;

            var strings = new List<string>() { "one", "two", "three" };
            var instances = new List<Database>();
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 3 };
            Parallel.ForEach(strings, options, instance =>
            {
                instances.Add(Database.Instance);
            });

            var log = Logger.Output();
            try
            {
                Assert.True(log.Count(log => log.Contains("Constructor")) > 1);
                Assert.Equal(3, log.Count(log => log.Contains("Instance")));
            }
            finally
            {
                Logger.Output().ToList().ForEach(h => _output.WriteLine(h));
            }
        }
    }
}