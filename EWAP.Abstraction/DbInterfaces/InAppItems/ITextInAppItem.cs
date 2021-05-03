namespace EWAP.Abstraction.DbInterfaces.InAppItems
{
    public interface ITextInAppItem : IInAppItem
    {
        public string HtmlText { get; set; }
    }
}
