using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Dao
{
    public class BackGround
    {
        private static BackGround backGround = new BackGround();

        public static BackGround getBackGround()
        {
            return backGround;
        }

        protected BackGround()
        {

        }

        public enum TaskType
        {
            PerbidTask,
            HistoryTask
        }

        public struct BackGroundTask {
            public string stockId;
            public TaskType taskType;
            public int cycleTime;
        }

        private Queue<Timer> timers = new Queue<Timer>();

        public void AddTask(BackGroundTask task)
        {
            switch (task.taskType)
            {
                case TaskType.PerbidTask:
                    if(task.cycleTime > 0){
                        timers.Enqueue(new Timer(Dao.DataSaver.SavePerbid, task.stockId, 0, task.cycleTime));
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(Dao.DataSaver.SavePerbid, task.stockId);
                    }
                    break;

                case TaskType.HistoryTask:
                    ThreadPool.QueueUserWorkItem(Dao.DataSaver.SaveHistory, task.stockId);
                    break;
            }
        }

        public void Execute(BackGroundTask task)
        {
            switch (task.taskType)
            {
                case TaskType.PerbidTask:
                    Dao.DataSaver.SavePerbid(task.stockId);
                    break;

                case TaskType.HistoryTask:
                    Dao.DataSaver.SaveHistory(task.stockId);
                    break;
            }
        }

        public void StopAllTasks()
        {
            int queueCount = timers.Count;
            for (int i = 0; i < queueCount; ++i)
            {
                timers.Dequeue().Dispose();
            }
        }
    }
}
