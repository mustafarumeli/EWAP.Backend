using System;

namespace EWAP.Abstraction.DbInterfaces.InAppItems
{
    public interface ITodoListElement
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDone { get; set; }
    }
}
