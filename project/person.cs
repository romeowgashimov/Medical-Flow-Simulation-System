using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using static project.Director;

namespace project
{
    public static class PersonTypes
    {
        public static readonly string[] Types = { "Бабушка", "Дедушка", "Женщина", "Мужчина" };
    }

    public class Person
    {
        private static int count;
        private int _id;
        private int _type;
        private int _time;
        
        public Person(int type, int time)
        {
            _id = count++;
            _type = (type >= 0 && type < PersonTypes.Types.Length) ? type : 0;
            _time = (time > 0 && time < 61) ? time : 1;
        }
        public Person() : this(0, 1) { }

        public override string ToString()
        {
            return PersonTypes.Types[_type] + " " + _id + ". Время " + _time;
        }

        public int ID 
        { 
            get { return _id; } 
        }
        public int Type 
        { 
            get { return _type; } 
        }
        public int Time 
        { 
            get { return _time; } 
        }
        public int UpdateTime(int time)
        {
            if (time < 0)
                return 0;
            int AddTime = time <= _time ? time : _time;
            _time -= AddTime;
            return AddTime;
        }

    }
}
