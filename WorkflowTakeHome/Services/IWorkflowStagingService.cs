using System.Text.Json;
using WorkflowTakeHome.Models;

namespace WorkflowTakeHome.Services
{
    public interface IWorkflowStagingService
    {
        WorkflowItem Stage(string sourceTool, JsonElement payload);
    }
}
