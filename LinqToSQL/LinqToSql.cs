using System;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace LinqToSQL
{
    public class LinqToSql
    {
        private DataContext dbContext;

        private static DataContext Connect(string sqlBaseLocation)
        {
            var dbConnection = new System.Data.SqlClient.SqlConnection(sqlBaseLocation);
            return new DataContext(dbConnection) {Log = Console.Out};
        }

        public LinqToSql(string location)
        {
            dbContext = Connect(location);
        }

        public void DeleteData()
        {
            var obj = dbContext.GetTable<Titles>().First(x => x.title_id == "DC1236");
            Console.WriteLine("{0} | {1} ", obj.title_id, obj.title);
            dbContext.GetTable<Titles>().DeleteOnSubmit(obj);
            dbContext.SubmitChanges();
            var res = dbContext.GetTable<Titles>().FirstOrDefault(x => x.title_id == "DC1236") ;
            Console.WriteLine(res);
            
        }

        public void UpdateData()
        {
            var data = dbContext.GetTable<Sale>().Take(1);
            Console.WriteLine("До изменения");
            Print(data);
            data.First().qty +=11;
            Console.WriteLine("После изменения");
            dbContext.SubmitChanges();
            Print(data);
        }

        public void FilterData()
        {
            var filtredData =
                from u in dbContext.GetTable<Authors>()
                where u.phone.StartsWith("415")
                select u;
            Print(filtredData);
        }

        private void Print<T>(IQueryable<T> query)
        {
            var parameters = typeof(T).GetProperties();
            foreach (var item in query)
            {
                Console.WriteLine(string.Join(" | ",
                    parameters.Select(x => typeof(T).GetProperty(x.Name)?.GetValue(item))));
            }
        }

        public void OrderData()
        {
            var orderedData = from u in dbContext.GetTable<Titles>()
                orderby u.price descending , u.title
                select new {Title = u.title, Title_Id = u.title_id, Price = u.price};
            Print(orderedData);
        }

        public void GroupData()
        {
            var groupedData = dbContext.GetTable<Titles>().GroupBy(x => x.type);
            foreach (var item in groupedData)
            {
                Console.WriteLine(item.Key);
                foreach (var i in item)
                {
                    Console.WriteLine("{0} | {1} | {2}", i.title_id,i.title,i.type);
                }
            }
        }

        public void PaggingData()
        {
            var pageData = dbContext.GetTable<Titleauthor>().Skip(5).Take(13);
            Print(pageData);
        }

        public void InsertData()
        {
            var newItem = new Titles() {title_id = "DC1235", title = "Torment with SQL"};
            dbContext.GetTable<Titles>().InsertOnSubmit(newItem);
            dbContext.SubmitChanges();
            var result = dbContext.GetTable<Titles>().First(x => x.title_id == "DC1235");
            Console.WriteLine(result.title_id);
        }
    }
}