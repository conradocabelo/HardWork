using System;
using System.Collections.Generic;
using HeavyWork.Entities;
using HeavyWork.Work;

namespace HeavyWork.Extensions
{
    public static class TestConfigurationExtension
    {
        public static List<TestDataCollected> Execute(this TestConfiguration testConfiguration, string testName, Action action) =>
           new MachineWork(testConfiguration).Work(testName, action);
    }
}
