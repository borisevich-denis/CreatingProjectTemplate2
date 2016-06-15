using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascon.Pilot.SDK.CreatingProjectTemplate
{

    public class ItemCB : PropertyChangedBase
    {
        public IDictionary<string,string> attr { get; set; }
        public string DispName { get; set; }
        public Guid Id { get; set; }

        public void Update(string DisplayName)
        {
            DispName = DisplayName;
            NotifyPropertyChanged("DispName");
        }
        public override string ToString()
        {
            return DispName;
        }
    }
}