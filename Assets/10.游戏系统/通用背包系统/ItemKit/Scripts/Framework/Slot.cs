namespace QFramework
{
    public class Slot
    {
        public IItem Item;
        public int Count;

        public Slot(IItem item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}