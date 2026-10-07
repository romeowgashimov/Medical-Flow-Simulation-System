using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project
{
    public delegate void AddLog(Log e);
    public class LogArgs : PrintArgs 
    { 
        public AddLog PrintLog; 
    }

    public class Log
    {
        private static int count;
        private int _id;
        private int _personid; 
        private int _type;
        private int _time;
        private int _timetoexit; //Поле необходимо для Linq запроса, обозначает прошёл ли полное обслуживание человек или нет

        public Log(int id, int type, int time, int timetoexit)
        {
            
            if (type >= 0 && type < PersonTypes.Types.Length && time > 0 && time < 61 && _personid >= 0) 
            {
                _id = count++;
                _personid = id;
                _type = type;
                _time = time;
                _timetoexit = timetoexit;
            }
            else
                throw new Exception("Параметры записи Log-файла заданы неправильно");
        }

        public override string ToString()
        {
            return _id + ": " + PersonTypes.Types[_type] + ", Время обслуживания " + _time;
        }

        public int ID 
        { 
            get { return _id; } 
        }
        public int PersonID 
        { 
            get { return _personid; } 
        }        
        public int Type 
        { 
            get { return _type; } 
        }
        public int Time 
        { 
            get { return _time; } 
        }
        public int TimeToExit 
        { 
            get { return _timetoexit; } 
        }
    }
}
