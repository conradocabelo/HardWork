using HeavyWork.Entities;

namespace HeavyWork.Persistence
{
    public interface IPersistence
    {
        void PersistData(TestDataCollected testDataCollected);
    }
}
