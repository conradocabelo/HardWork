using System;
using System.Diagnostics;
using HeavyWork.Entities;

namespace HeavyWork.Work
{
    public class WorkerTest
    {
        private readonly string _testLabel;
        private readonly Action _action;

        public WorkerTest(string TestLabel, Action action)
        {
            _testLabel = TestLabel;
            _action = action;
        }

        public TestDataCollected ExecuteWork(TestDataCollected testDataCollected)
        {
            var stopwatch = new Stopwatch();

            testDataCollected.StartTest = DateTime.Now;

            try
            {
                stopwatch.Start();

                _action.Invoke();

                stopwatch.Stop();

                testDataCollected.Status = StatusTest.Sucess;
            }
            catch (Exception error)
            {
                if (stopwatch.IsRunning)
                    stopwatch.Stop();

                testDataCollected.ErrorMessage = error.InnerException.Message;
                testDataCollected.Status = StatusTest.Error;
            }
            finally
            {
                testDataCollected.EndTest = DateTime.Now;
            }

            testDataCollected.TestLabel = _testLabel;
            testDataCollected.TimeElapsed = stopwatch.Elapsed;

            return testDataCollected;
        }
    }
}
