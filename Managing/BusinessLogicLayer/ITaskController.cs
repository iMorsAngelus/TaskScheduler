using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Managing.BusinessLogicLayer
{
    /// <summary>
    /// Represent an interface of ITaskController
    /// </summary>
    interface ITaskController
    {
        /// <summary>
        /// Load tasks from file.
        /// </summary>
        /// <param name="path">Path to source.</param>
        /// <returns>Collection of tasks.</returns>
        Task<ObservableCollection<ScheduleTask>> LoadTasks(string path);
        /// <summary>
        /// Add task to collection.
        /// </summary>
        /// <param name="scheduleTask">Task for adding.</param>
        /// <returns>Collection of tasks.</returns>
        ObservableCollection<ScheduleTask> AddTask(ScheduleTask scheduleTask);
        /// <summary>
        /// Remove task from collection.
        /// </summary>
        /// <param name="scheduleTask">Task for remove.</param>
        /// <returns>Collection of tasks.</returns>
        ObservableCollection<ScheduleTask> RemoveTask(ScheduleTask scheduleTask);
        /// <summary>
        /// Edit concreate task in collection.
        /// </summary>
        /// <param name="index">Task index.</param>
        /// <param name="scheduleTask">Updated task.</param>
        /// <returns>Collection of tasks.</returns>
        ObservableCollection<ScheduleTask> EditTask(int index, ScheduleTask scheduleTask);
        /// <summary>
        /// Processing every task.
        /// </summary>
        /// <returns>Task with processing.</returns>
        Task ProcessingTasks();
    }
}
