using System;
using System.Collections.Generic;
using HeavyWork.Entities;
using HeavyWork.Persistence;
using HeavyWork.Work;

namespace HeavyWork.Extensions
{
    public static class TestConfigurationExtension
    {
        public static List<TestDataCollected> Execute(this TestConfiguration testConfiguration, string indexName, string testName, Action action) =>
           new MachineWork(indexName, testConfiguration).Work(testName, action);

        public static IPersistence CreatePersistence(this TestConfiguration testConfiguration)
        {
            switch (testConfiguration.TypePersistent)
            {
                case TypePersistent.SqlServer:
                    return new SqlServerPersistence(testConfiguration);
                default:
                    return null;
            }
        }
    }
}
