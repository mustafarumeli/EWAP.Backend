namespace EWAP.Abstraction.DbInterfaces
{
    public interface IGeneralItems : IItem
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
    }

}
