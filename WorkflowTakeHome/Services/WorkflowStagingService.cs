using System.Text.Json;
using WorkflowTakeHome.Models;
using WorkflowTakeHome.Services.Parsers;

namespace WorkflowTakeHome.Services
{
    public class WorkflowStagingService : IWorkflowStagingService
    {
        private readonly IGenericDataParser _parser;
        private readonly HashSet<string> _allowedTools = ["ToolA", "ToolB"];
        
        public WorkflowStagingService(IGenericDataParser parser)
        {
            _parser = parser;
        }

        public WorkflowItem Stage(string sourceTool, JsonElement payload)
        {
            if (string.IsNullOrWhiteSpace(sourceTool))
                throw new ArgumentException("Source tool is required.");

            if (!_allowedTools.Contains(sourceTool))
                throw new ArgumentException($"Source tool '{sourceTool}' is not recognized.");

            if (payload.ValueKind != JsonValueKind.Object || payload.GetRawText() == "{}")
                throw new ArgumentException("Payload is empty or invalid JSON.");

            // validate required fields
            if (!payload.TryGetProperty("respondent", out var respondentProp))
                throw new ArgumentException("Missing required field: 'respondent'.");

            if (respondentProp.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(respondentProp.GetString()))
                throw new ArgumentException("Field 'respondent' must be a non-empty string.");

            var item = _parser.Parse(sourceTool, payload);

            SaveToStorage(item); 
            return item;
        }

        private void SaveToStorage(WorkflowItem item)
        {
            //Logic to save to the db
            Console.WriteLine($"Saved: {item.SourceTool} | {item.Id}");
        }
    }

}
