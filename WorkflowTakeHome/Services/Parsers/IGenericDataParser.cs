using System.Text.Json;
using WorkflowTakeHome.Models;

namespace WorkflowTakeHome.Services.Parsers
{
    public interface IGenericDataParser
    {
        WorkflowItem Parse(string sourceTool, JsonElement json);
    }
}
