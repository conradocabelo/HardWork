using System;

namespace HeavyWork.Entities
{
    public class TestDataCollected
    {
        public string TestLabel { get; set; }

        public DateTime StartTest { get; set; }
        public DateTime EndTest { get; set; }

        public TimeSpan TimeElapsed { get; set; }

        public StatusTest Status { get; set; }
        public string ErrorMessage { get; set; }

        public TestDataCollected() =>
            Status = StatusTest.NotExecuting;
    }
}
