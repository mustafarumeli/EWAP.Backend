using System.Collections.Generic;

namespace EWAP.Abstraction.DbInterfaces.InAppItems
{
    public interface IToDoListInAppItem : IInAppItem
    {
        public string Title { get; set; }
        IEnumerable<ITodoListElement> Items { get; set; }
    }
}
