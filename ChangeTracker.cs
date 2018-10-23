using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Linq
{
    class ChangeTracker
    {
        private List<ChangeTrackerEntry> _entries = new List<ChangeTrackerEntry>();
        private enum States
        {
            Unmodified,
            Modified,
            Deleted,
            Added
        }

        public void AddInserted(object obj)
        {
            var cte = new ChangeTrackerEntry();
            cte.State = States.Added;
            cte.Entry = obj;

            _entries.Add(cte);
        }

        public void AddUnmodified(object obj)
        {
            var cte = new ChangeTrackerEntry();
            cte.State = States.Unmodified;
            cte.Entry = obj;

            var t = obj.GetType();
            foreach (var prop in t.GetProperties())
            {
                cte.Originals.Add(new Tuple<PropertyInfo, object>(prop, prop.GetValue(obj)));
            }

            _entries.Add(cte);
        }

        public void SetDeleted(object obj)
        {
            _entries.Single(i => i.Entry == obj).State = States.Deleted;
        }

        private class ChangeTrackerEntry
        {
            public States State { get; set; }
            public object Entry { get; internal set; }
            public List<Tuple<PropertyInfo, object>> Originals { get; set; }
        }
    }
}
