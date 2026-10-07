using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static project.PersonQueue;

namespace project
{
    public class NonPriorQueue
    {
        private Stack<Person> _nonpriorqueue = new Stack<Person>();
        private ListBox _lb;
        private string _name = "Отложенная очередь";

        public NonPriorQueue(ListBox lb) 
        { 
            _lb = lb; 
        }

        public NonPriorQueue() : this(null) { }

        public override string ToString() 
        { 
            return _name; 
        }

        //Метод добавления в отложенную очередь, Обработчик события "Замена"
        public void NewPerson(object sender, DirectorArgs e)
        {
            if (e._person != null)
            {
                _nonpriorqueue.Push(e._person);
                _lb?.Items.Add(e._person);
                e.PrintResult?.Invoke(this + ": " + e._person + "добавлен человек");
                e._person = null;
            }
        }

        public void CheckTypew(object sender, DirectorArgs e)
        {
            if (_nonpriorqueue.Count > 0)
                if (_nonpriorqueue.First<Person>().Type < e._type)
                {
                    e._queuemethod = SetDirector;
                    e._type = _nonpriorqueue.First<Person>().Type;
                }
        }

        public void SetDirector(DirectorArgs e)
        {
            e._person = _nonpriorqueue.Pop();
            _lb.Items.Remove(e._person);
            e.PrintResult?.Invoke(this + ": " + e._person + " ушёл к начальнику");
        }

        public void Clear() 
        { 
            _nonpriorqueue.Clear(); 
            _lb.Items.Clear(); 
        }
    }
}
