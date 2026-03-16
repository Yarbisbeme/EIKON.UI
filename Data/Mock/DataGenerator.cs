using Bogus;
using System.Reflection;

//namespace EIKON.UI.Data.Mock
//{
//    public class DataGenerator
//    {
//    }
//}
//using Bogus;
//using System.Reflection;

namespace BlazorApp.Data.Mock;

public static class MockDataGenerator
{
    public static List<T> Generate<T>(int count = 9) where T : class, new()
    {
        var faker = new Faker<T>("es");

        var type = typeof(T);
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var name = prop.Name.ToLower();
            if (prop.PropertyType == typeof(string))
            {
                if (name.Contains("nombre") || name.Contains("title"))
                    faker.RuleFor(prop.Name, f => f.Commerce.ProductName());
                else if (name.Contains("email"))
                    faker.RuleFor(prop.Name, f => f.Internet.Email());
                else if (name.Contains("direccion") || name.Contains("address"))
                    faker.RuleFor(prop.Name, f => f.Address.FullAddress());
                else if (name.Contains("codigo") || name.Contains("Codigo"))
                    faker.RuleFor(prop.Name, f => "00"+ f.IndexFaker.ToString());
                else if (name.Contains("maenume") || name.Contains("maenomi"))
                    faker.RuleFor(prop.Name, f => "0000000" + f.IndexFaker.ToString());
                else
                    faker.RuleFor(prop.Name, f => f.Lorem.Word());
            }
            else if (prop.PropertyType == typeof(int))
            {
                faker.RuleFor(prop.Name, f => f.Random.Int(1, 1000));
            }
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(double))
            {
                faker.RuleFor(prop.Name, f => f.Random.Decimal(1, 5000));
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                faker.RuleFor(prop.Name, f => f.Date.Past(2));
            }
            else if (prop.PropertyType == typeof(bool))
            {
                faker.RuleFor(prop.Name, f => f.Random.Bool());
            }
        }

        return faker.Generate(count);
    }
}

