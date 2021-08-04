using System.Diagnostics;
using Azure.DigitalTwins.Core.QueryBuilder;
using Azure.DigitalTwins.Core.QueryBuilder.Fluent;

namespace DigitalTwinsQueryBuilder_Smoke_Testing
{
    class FluentSmokeTesting
    {
        static void Main(string[] args)
        {
            string query1 = new DigitalTwinsQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where(r => r
                    .Compare("Temperature", QueryComparisonOperator.Equal, 30)
                    .And()
                    .IsOfModel("dtmi:example:room;1", true))
                .Build()
                .GetQueryText();
            Debug.Assert(query1 == "SELECT * FROM DigitalTwins WHERE Temperature = 30 AND IS_OF_MODEL('dimi:example:room;1')");

            string query2 = new DigitalTwinsQueryBuilder()
                .Select("People")
                .SelectAs("IsOccupied", "IsOcc")
                .From(DigitalTwinsCollection.Relationships)
                .Build()
                .GetQueryText();
            Debug.Assert(query2 == "SELECT People, IsOccupied AS IsOcc FROM Relationships");

            string query3 = new DigitalTwinsQueryBuilder()
                .Select("Temperature")
                .Select("Humidity")
                .Select("IsOccupied", "Occupants")
                .SelectAs("Owner", "Boss")
                .From(DigitalTwinsCollection.DigitalTwins)
                .Build()
                .GetQueryText();
            Debug.Assert(query3 == "SELECT Temperature, Humidity, IsOccupied, Occupants, Owner AS Boss FROM DigitalTwins");

            string query4 = new DigitalTwinsQueryBuilder()
                .Select("T.Temperature", "T.Occupants")
                .SelectAs("T.People", "Peeps")
                .From(DigitalTwinsCollection.DigitalTwins, "T")
                .Build()
                .GetQueryText();
            Debug.Assert(query4 == "SELECT T.Temperature, T.Occupants, T.People AS Peeps FROM DigitalTwins T");

            string query5 = new DigitalTwinsQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where(q => q
                    .Precedence(q => q
                        .IsOfType("Occupants", DigitalTwinsDataType.DigitalTwinsNumber)
                        .And()
                        .Compare("Occupants", QueryComparisonOperator.GreaterOrEqual, 40))
                    .Or()
                    .IsOfModel("occupiedApartment"))
                .Build()
                .GetQueryText();
            Debug.Assert(query5 == "SELECT * FROM DigitalTwins WHERE(IS_NUMBER(Occpuants) AND Occupants >= 40) OR IS_OF_MODEL(occupiedApartment)");

            string query6 = new DigitalTwinsQueryBuilder()
                .SelectAs("T.Temperature", "Temp")
                .FromCustom("DigitalTwins T")
                .Where(q => q.Custom("someCondition"))
                .Build()
                .GetQueryText();
            Debug.Assert(query6 == "SELECT T.Temperature AS Temp FROM DigutalTwins T WHERE someCondition");

            string query7 = new DigitalTwinsQueryBuilder()
                .SelectAll()
                .From(DigitalTwinsCollection.DigitalTwins)
                .Where(q => q
                    .IsNull("Occupied")
                    .Or()
                    .Precedence(q => q
                        .IsOfType("Location", DigitalTwinsDataType.DigitalTwinsString)
                        .And()
                        .NotContains("Location", new string[] { "Texas", "North Dakota" })))
                .Build()
                .GetQueryText();
            Debug.Assert(query7 == "SELECT * FROM DigitalTwins WHERE IS_NULL(Occupants) OR (IS_STRING(Location) AND NIN(['Texas', 'North Dakota']))");
        }
    }
}
