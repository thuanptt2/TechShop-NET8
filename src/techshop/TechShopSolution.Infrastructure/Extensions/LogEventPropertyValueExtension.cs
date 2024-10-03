using Serilog.Events;

namespace TechShopSolution.Infrastructure.Extensions
{
    public static class LogEventPropertyValueExtension
    {
        public static object? ExtractValue(this LogEventPropertyValue propertyValue)
        {
            try
            {
                if(propertyValue == null)
                {
                    return null;
                }

                switch (propertyValue)
                {
                    case ScalarValue scalarValue:
                    {
                        return scalarValue.Value;
                    }
                    // case SequenceValue sequenceValue:
                    // {
                    //     var list = new List<object?>();
                    //     foreach (var item in sequenceValue.Elements)
                    //     {
                    //         list.Add(ExtractValue(item));
                    //     }
                    //     return list;
                    // }
                    // case StructureValue structureValue:
                    // {
                    //     var dict = new Dictionary<string, object?>();
                    //     foreach (var property in structureValue.Properties)
                    //     {
                    //         dict[property.Name] = ExtractValue(property.Value);
                    //     }
                    //     return dict;
                    // }
                    // case DictionaryValue dictionaryValue:
                    // {
                    //     var expando = new ExpandoObject();
                    //     var expandoDict = (IDictionary<string, object?>)expando;
                    //     foreach (var kvp in dictionaryValue.Elements)
                    //     {
                    //         //var key = ExtractValue(kvp.Key)?.ToString();
                    //         var key = kvp.Key.ToString();
                    //         var value = ExtractValue(kvp.Value);
                    //         expandoDict[key] = value;
                    //     }
                    //     return expando;
                    // }
                    default:
                    {
                        return propertyValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExtractValueFail: " + ex.Message);
            }

            return null;
        }
    }
}