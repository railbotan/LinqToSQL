namespace LinqToSQL
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var e = new LinqToSql(@"Server=(local);Integrated Security=true");
            e.DeleteData();
        }
    }
}