using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavyWork.Entities;
using HeavyWork.Extensions;
using HeavyWork.Persistence;

namespace HeavyWork.Work
{
    public class MachineWork
    {
        private readonly TestConfiguration _configuration;
        private readonly IPersistence _persistence;
        private readonly string _indexName;

        public MachineWork(string indexName, TestConfiguration testConfiguration) 
        {
            _configuration = testConfiguration;
            _indexName = indexName;
            _persistence = _configuration.CreatePersistence();
        }
         
        public List<TestDataCollected> Work(string TestName, Action action)
        {
            if (_configuration.NumberThreads == 1)
                return ExecuteSingleThread(TestName, action);
            else
                return ExecuteMultThread(TestName, action);
        }

        private List<TestDataCollected> ExecuteMultThread(string TestName, Action action)
        {
            List<Task<List<TestDataCollected>>> testDataCollecteds = Enumerable.Range(0, _configuration.NumberThreads)
                                                                               .Select(t => CreateAsync(TestName, action)).ToList();

            var ResultTask = Task.WhenAll(testDataCollecteds.ToArray()).Result.ToList();

            List<TestDataCollected> ReturnData = new List<TestDataCollected>();
            ResultTask.ForEach(t => ReturnData.AddRange(t));

            return ReturnData;
        }

        private async Task<List<TestDataCollected>> CreateAsync(string TestName, Action action) =>
            ExecuteSingleThread(TestName, action);

        private List<TestDataCollected> ExecuteSingleThread(string TestName, Action action)
        {
            List<TestDataCollected> dataCollected = Enumerable.Range(0, _configuration.NumberRequests)
                                                              .Select(t => new TestDataCollected()).ToList();

            return dataCollected.Select(t => {
                var data = new WorkerTest(TestName, action).ExecuteWork(t);
                Persist(t);
                return data;
            }).ToList();
        }

        private void Persist(TestDataCollected testDataCollected)
        {
            if(this._configuration.TypePersistent != TypePersistent.NoPersist)
                _persistence.PersistData(testDataCollected);
        }
    }
}
