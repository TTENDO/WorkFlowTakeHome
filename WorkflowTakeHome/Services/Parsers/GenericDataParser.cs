using System.Text.Json;
using WorkflowTakeHome.Models;

namespace WorkflowTakeHome.Services.Parsers
{
    public class GenericDataParser : IGenericDataParser
    {
        public WorkflowItem Parse(string sourceTool, JsonElement json)
        {
            var dict = new Dictionary<string, object>();
            FlattenJson(json, dict, "");

            return new WorkflowItem
            {
                Id = Guid.NewGuid(),
                SourceTool = sourceTool,
                NormalizedData = dict,
                Status = WorkflowStatus.PendingReview,
                ReceivedAt = DateTime.UtcNow
            };
        }

        private void FlattenJson(JsonElement element, Dictionary<string, object> result, string prefix)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in element.EnumerateObject())
                    FlattenJson(prop.Value, result, $"{prefix}{prop.Name}.");
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                int i = 0;
                foreach (var item in element.EnumerateArray())
                    FlattenJson(item, result, $"{prefix}[{i++}].");
            }
            else
            {
                result[prefix.TrimEnd('.')] = ExtractValue(element);
            }
        }

        private object? ExtractValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.TryGetInt32(out var i) ? i : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => element.ToString()
            };
        }
    }
}
