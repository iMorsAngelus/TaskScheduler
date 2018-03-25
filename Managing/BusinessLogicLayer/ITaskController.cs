using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Managing.DataAccessLayer;

namespace Managing.BusinessLogicLayer
{
    interface ITaskController
    {
        Task<ObservableCollection<ScheduleTask>> LoadTasks(string path);
        ObservableCollection<ScheduleTask> AddTask(ScheduleTask scheduleTask);
        ObservableCollection<ScheduleTask> RemoveTask(ScheduleTask scheduleTask);
        ObservableCollection<ScheduleTask> EditTask(int index, ScheduleTask scheduleTask);
    }
}
