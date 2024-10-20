using BenchmarkDotNet.Running;
using Bogus;
using CreateCsvFileConsole;
using SourceGeneratorVsReflection;
using SourceGeneratorVsReflection.Models;
using SourceGeneratorVsReflection.ReflectionTest;
using SourceGeneratorVsReflection.SourceGeneratorTest;
using System.Diagnostics;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
var faker = new Faker<RandomPropertiesClass>()
    .RuleFor(r => r.Name, f => f.Name.FirstName())
    .RuleFor(r => r.Age, f => f.Random.Int(18, 80))
    .RuleFor(r => r.Height, f => f.Random.Double(150, 200))
    .RuleFor(r => r.Surname, f => f.Name.LastName())
    .RuleFor(r => r.HouseNumber, f => f.Random.Int(1, 1000))
    .RuleFor(r => r.Weight, f => f.Random.Double(40, 120))
    .RuleFor(r => r.Address, f => f.Address.StreetAddress())
    .RuleFor(r => r.TaxCode, f => f.Random.Int(10000, 99999))
    .RuleFor(r => r.Salary, f => f.Random.Double(20000, 100000))
    .RuleFor(r => r.City, f => f.Address.City())
    .RuleFor(r => r.ZipCode, f => f.Random.Int(10000, 99999))
    .RuleFor(r => r.DiscountPercentage, f => f.Random.Double(0, 0.5))
    .RuleFor(r => r.Country, f => f.Address.Country())
    .RuleFor(r => r.PhoneNumber, f => f.Random.Int(100000000, 999999999))
    .RuleFor(r => r.Latitude, f => f.Random.Double(-90, 90))
    .RuleFor(r => r.Email, f => f.Internet.Email())
    .RuleFor(r => r.YearsOfExperience, f => f.Random.Int(0, 40))
    .RuleFor(r => r.Longitude, f => f.Random.Double(-180, 180))
    .RuleFor(r => r.Profession, f => f.Name.JobTitle())
    .RuleFor(r => r.NumberOfChildren, f => f.Random.Int(0, 5))
    .RuleFor(r => r.Temperature, f => f.Random.Double(-10, 40))
    .RuleFor(r => r.FavoriteColor, f => f.Commerce.Color())
    .RuleFor(r => r.ShoeSize, f => f.Random.Int(35, 47))
    .RuleFor(r => r.PurchasePrice, f => f.Random.Double(10, 1000))
    .RuleFor(r => r.Hobby, f => f.Lorem.Word())
    .RuleFor(r => r.LicenseNumber, f => f.Random.Int(100000, 999999))
    .RuleFor(r => r.AverageRating, f => f.Random.Double(1, 5))
    .RuleFor(r => r.SpokenLanguage, f => f.Lorem.Word())
    .RuleFor(r => r.RoomNumber, f => f.Random.Int(1, 500))
    .RuleFor(r => r.Balance, f => f.Random.Double(-1000, 10000))
    .RuleFor(r => r.SubscriptionType, f => f.PickRandom(new[] { "Free", "Basic", "Premium" }))
    .RuleFor(r => r.LoyaltyPoints, f => f.Random.Int(0, 1000))
    .RuleFor(r => r.DistanceTraveled, f => f.Random.Double(0, 10000))
    .RuleFor(r => r.Nationality, f => f.Address.Country())
    .RuleFor(r => r.WarrantyYears, f => f.Random.Int(1, 5))
    .RuleFor(r => r.EnergyConsumption, f => f.Random.Double(50, 500))
    .RuleFor(r => r.MaritalStatus, f => f.PickRandom(new[] { "Single", "Married", "Divorced", "Widowed" }))
    .RuleFor(r => r.OrderNumber, f => f.Random.Int(1000, 9999));

List<RandomPropertiesClass> randomData = faker.Generate(10_000);

// Stampa alcuni dati per verifica
foreach (var item in randomData.Take(20))
{
    Console.WriteLine($"Name: {item.Name}, Age: {item.Age}, Profession: {item.Profession}, City: {item.City}");
}

string csvFilePath = Configuration.GetCsvFileNameWithPath;

// Write the data to csv
using (StreamWriter writer = new StreamWriter(csvFilePath))
{
    // Header
    writer.WriteLine(string.Join(",", typeof(RandomPropertiesClass).GetProperties().Select(p => p.Name)));

    // Write the data
    foreach (var item in randomData)
    {
        var values = typeof(RandomPropertiesClass).GetProperties()
            .Select(p =>
            {
                var value = p.GetValue(item)?.ToString() ?? "";
                // manage comma and double quote
                return value.Contains(',') ? $"\"{value.Replace("\"", "\"\"")}\"" : value;
            });
        writer.WriteLine(string.Join(",", values));
    }
}

Console.WriteLine($"File created: {Path.GetFullPath(csvFilePath)}");

#if(!DEBUG)
var benchmarkSummary1 = BenchmarkRunner.Run<BenchmarkTest>();
return;
#endif

Console.WriteLine(new string('=', 120));

Stopwatch sw1 = Stopwatch.StartNew();
MapWithReflection.Import();
sw1.Stop();
Console.WriteLine($"Reflection import = {sw1.ElapsedMilliseconds}ms");

Console.WriteLine();

Stopwatch sw2 = Stopwatch.StartNew();
MapWithSourceGenerator.Import();
sw2.Stop();
Console.WriteLine($"Source Generator import = {sw2.ElapsedMilliseconds}ms");
