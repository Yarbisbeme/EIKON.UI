//using Bogus;
//using EIKON.UI.Interfaces;
//using System.Reflection;

//namespace EIKON.UI.Data.Mock
//{
//    public class MockService
//    {
//    }
//}

using Bogus;
using EIKON.UI.Interfaces;
using System.Reflection;

namespace EIKON.UI.Data.Mock;

public class MockService<T> : IMockService<T> where T : class, new()
{
    public Task<List<T>> GetAllAsync(int count = 9)
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
                else if (name.Contains("telefono") || name.Contains("phone"))
                    faker.RuleFor(prop.Name, f => f.Phone.PhoneNumber());
                else if (name.Contains("codigo") )
                    faker.RuleFor(prop.Name, f => "00" + (f.IndexFaker + 1).ToString());
                else if (name.Contains("nume") || name.Contains("maenomi") || name.Contains("number"))
                    faker.RuleFor(prop.Name, f => "00000" + (f.IndexFaker +1).ToString());
                else if (name.Contains("descri") || name.Contains("description"))
                    faker.RuleFor(prop.Name, f => prop.Name + " Eikon " + (f.IndexFaker + 1).ToString());
                else
                    faker.RuleFor(prop.Name, f => prop.Name + " Eikon " + f.Lorem.Word());
            }
            else if (prop.PropertyType == typeof(int))
            {
                faker.RuleFor(prop.Name, f => f.IndexFaker+1);
            }
            else if (prop.PropertyType == typeof(decimal))
            {
                faker.RuleFor(prop.Name, f => f.Random.Decimal(1, 50));
            }
            else if (prop.PropertyType == typeof(double))
            {
                faker.RuleFor(prop.Name, f => Math.Round(f.Random.Double(1, 500), 2));
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

        var data = faker.Generate(count);
        return Task.FromResult(data);
    }
}
