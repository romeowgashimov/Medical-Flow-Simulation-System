using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public delegate void HistoryLog(string s);

    public class PrintArgs : EventArgs 
    { 
        public HistoryLog PrintResult; 
    }


    internal class Manager
    {
        private List<PersonQueue> PersonQueues = new List<PersonQueue>();
        private List<Log> LogList = new List<Log>();
        private Random rnd = new Random();
        private int luck = 2;

        private TextBox Log_TB, Stat_TB;
        public Manager(TextBox textBox, TextBox StatBox) 
        { 
            Log_TB = textBox; 
            Stat_TB = StatBox; 
        }
        public Manager() : this(null, null) { }

        private event EventHandler<DirectorArgs> New;
        private void OnNew()
        {
            DirectorArgs e = new DirectorArgs();
            e._person = new Person(rnd.Next(PersonTypes.Types.Length), rnd.Next(1, 6));
            e.PrintResult = PrintResult;
            New(this, e);
        }

        private event EventHandler<LogArgs> Run;
        private event Action Clear;
        private void OnRun()
        {
            if (Run != null)
            {
                LogArgs e = new LogArgs();
                e.PrintResult = PrintResult;
                e.PrintLog = PrintLog;
                Run(this, e);
            }
        }

        private void PrintResult(string s)
        {
            if (Log_TB != null)
                Log_TB.AppendText(s + Environment.NewLine);
        }
        private void PrintLog(Log s)
        {
            LogList.Add(s);
        }
        public void OnTimer()
        {
            Log_TB.Clear();
            OnRun();
            if (rnd.Next(luck) == 0)
                OnNew();
        }
        public void AddQueue(PersonQueue q, Director dir)
        {
            PersonQueues.Add(q);
            New += q.NewPerson;
            dir.IsReady += q.CheckTypew;
            q.FirstPerson += dir.WaitFirst;
            Clear += q.Clear;
        }
        public void RemoveQueue(PersonQueue q, Director dir)
        {
            PersonQueues.Remove(q);
            New -= q.NewPerson;
            dir.IsReady -= q.CheckTypew;
        }
        public void AddDir(Director dir)
        {
            Run += dir.RunTime;
            Clear += dir.Clear;
        }
        public void AddQueue4(NonPriorQueue q, Director dir)
        {
            dir.IsChange += q.NewPerson;
            dir.IsReady += q.CheckTypew;
            Clear += q.Clear;
        }

        public void RemoveQueue4(NonPriorQueue q, Director dir)
        {
            dir.IsChange -= q.NewPerson;
            dir.IsReady -= q.CheckTypew;
            Clear -= q.Clear;
        }

        public void SetStatistic()
        {
            var result = from t in PersonTypes.Types 
                         join l in LogList
                         on t equals PersonTypes.Types[l.Type] into tl 
                         orderby t 
                         select new
                         {
                             Type = t, 
                             Count = (from p in tl
                                      where p.TimeToExit == 1
                                      select p).Count(),
                             Sum = (from p in tl
                                    group p by p.PersonID into tlp
                                    where tlp.Last().TimeToExit == 1
                                    select new {
                                    s = tlp.First().TimeToExit
                                    }).Sum(x => x.s)
                         };
            Stat_TB.Clear();
            foreach (var elem in result)
                Stat_TB.AppendText(Environment.NewLine + "Категория: " + elem.Type + Environment.NewLine + "  Количество принятый граждан: "
                                    + elem.Count + Environment.NewLine + "  Суммарное время приёмов: " + elem.Sum);
        }

        public void OnClear()
        {
            Log_TB.Clear(); Stat_TB.Clear();
            LogList.Clear(); PersonQueues.Clear();
            Clear();
        }

    }
}
