using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public class PersonQueue
    {
        private static int count;
        private int _id;
        private int _type;
        private Queue<Person> _queue = new Queue<Person>();
        private ListBox _lb;
        private string _name;

        public PersonQueue(int type, ListBox lb)
        {
            _id = count++;
            _type = (type >= 0 && type < PersonTypes.Types.Length) ? type : 0;
            _lb = lb;
            switch (_type)
            {
                case 0:
                    _name = "Очередь бабушек ";
                    break;
                case 1:
                    _name = "Очередь дедушек ";
                    break;
                case 2:
                    _name = "Очередь женщин ";
                    break;
                case 3:
                    _name = "Очередь мужчин ";
                    break;
            }
        }
        public PersonQueue() : this(0, null) { }

        public override string ToString() 
        { 
            return _name; 
        }

        public int QueueID 
        { 
            get { return _id; } 
        }
        public int QueueType 
        { 
            get { return _type; } 
        }

        public event EventHandler<DirectorArgs> FirstPerson;       

        private void OnFirstPerson(HistoryLog PrintResult)
        {
            if (FirstPerson != null && _queue.Count == 1)
            {
                DirectorArgs e = new DirectorArgs
                {
                    PrintResult = PrintResult,
                    _queuemethod = SetDirector, 
                    _person = _queue.First()
                };
                FirstPerson(this, e);
            }
        }

        public void NewPerson(object sender, DirectorArgs e)
        {
            if (e._person != null)
                if (_type == e._person.Type)
                {
                    _queue.Enqueue(e._person);
                    _lb?.Items.Add(e._person);
                    e.PrintResult?.Invoke(this + ": добавлен человек " + e._person);
                    e._person = null;
                    OnFirstPerson(e.PrintResult);
                }
        }


        public void CheckTypew(object sender, DirectorArgs e)
        {
            if (_type < e._type && _queue.Count > 0)
            {
                e._queuemethod = SetDirector;
                e._type = _type;
            }
        }

        public void SetDirector(DirectorArgs e)
        {
            e._person = _queue.Dequeue(); 
            _lb.Items.Remove(e._person);
            e.PrintResult?.Invoke(this + ": " + e._person + " ушёл к начальнику");
        }

        public void Clear() 
        { 
            _queue.Clear();
            _lb.Items.Clear(); 
            count = 0; 
        }
    }
}
