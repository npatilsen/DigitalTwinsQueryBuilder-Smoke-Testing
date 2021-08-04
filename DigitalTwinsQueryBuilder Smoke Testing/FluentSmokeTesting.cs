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
            Debug.Assert(query1 == "SELECT * FROM DigitalTwins WHERE Temperature = 30 AND IS_OF_MODEL('dimi:example:room;1' exact)");

            string query2 = new DigitalTwinsQueryBuilder()
                .Select("People")
                .SelectAs("IsOccupied", "IsOcc")
                .From(DigitalTwinsCollection.Relationships)
                .Build()
                .GetQueryText();
            Debug.Assert(query2 == "SELECT People, IsOccupied AS IsOcc FROM Relationships");
        }
    }
}
