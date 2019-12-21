using CsvHelper.Configuration;

namespace Tests
{
    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Map(p => p.Name)
                .Name("NAME");

            Map(p => p.Salary)
                .Name("SALARY")
                .ConvertUsing(row => IntelligentCurrencyConverter.ConvertCurrency(row.GetField("SALARY")));
        }
    }
}
