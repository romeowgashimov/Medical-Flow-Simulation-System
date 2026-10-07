using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public delegate void DirectorLog(DirectorArgs e);

    public class DirectorArgs : PrintArgs 
    { 
        public int _type; 
        public DirectorLog _queuemethod; 
        public Person _person; 
    }
    public class Director
    {
        private TextBox _tb;
        private Person _person; //Дополнительных полей не требуется, так как начальник один и обслуживает одного гражданина

        public Director(TextBox tb)
        {
            _tb = tb;
            if (_tb != null)
                tb.Clear();
        }
        public Director() : this(null) { }

        public override string ToString() { return _person.ToString(); }
        private void SetPerson(DirectorArgs e)
        {
            if (e._person != null)
            {
                _person = e._person;
                if (_tb != null)
                    _tb.Text = e._person.ToString();
                e.PrintResult?.Invoke(this + ": обслуживается у начальника ");
            }
        }
        
        public event EventHandler<DirectorArgs> IsReady;
        public event EventHandler<DirectorArgs> IsChange;

        private void OnIsReady(HistoryLog PrintResult)
        {
            if (IsReady != null && _person == null)
            {
                DirectorArgs e = new DirectorArgs
                { 
                    PrintResult = PrintResult, 
                    _type = 4 
                };
                IsReady?.Invoke(this, e);  
                if (e._queuemethod != null)
                { 
                    e._queuemethod(e); 
                    SetPerson(e); 
                }
            }
        }

        public void WaitFirst(object sender, DirectorArgs e)
        {
            if (_person != null) 
            {
                if (e._person.Type < _person.Type) 
                {
                    e.PrintResult?.Invoke(this + ": " + "уходит в отложенную очередь");
                    IsChange?.Invoke(this, new DirectorArgs() { _person = this._person, PrintResult = e.PrintResult });                                       
                    if (e._queuemethod != null)
                    {
                        e._queuemethod(e);
                        SetPerson(e); 
                    }                   
                }
            }     
            else
            {
                if (e._queuemethod != null)
                {
                    e._queuemethod(e); 
                    SetPerson(e); 
                }
            }
        }

        public void RunTime(object sender, LogArgs e)
        {
            if (_person != null)
            {
                int t = _person.UpdateTime(1);
                e.PrintResult?.Invoke(this + ": " + " обслужился " + t + " минут");
                e.PrintLog?.Invoke(new Log(_person.ID, _person.Type, t, _person.Time + t));
                if (_person.Time == 0)
                {
                    _tb?.Clear();
                    e.PrintResult?.Invoke(this + ": ушёл.");
                    _person = null;
                    OnIsReady(e.PrintResult);
                }
                else
                {
                    if (_tb != null)
                        _tb.Text = _person.ToString();
                }
            }
        }

        public void Clear() 
        { 
            _person = null;
            _tb.Clear();
        }

    }
}
