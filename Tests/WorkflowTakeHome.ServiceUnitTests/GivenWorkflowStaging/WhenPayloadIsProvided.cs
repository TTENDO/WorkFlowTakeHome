using Moq;
using System.Text.Json;
using WorkflowTakeHome.Models;
using WorkflowTakeHome.Services;
using WorkflowTakeHome.Services.Parsers;

namespace WorkflowTakeHome.ServiceUnitTests.GivenWorkflowStaging
{
    public class WhenPayloadIsProvided
    {
        private readonly Mock<IGenericDataParser> _mockParser;
        private readonly WorkflowStagingService _service;

        public WhenPayloadIsProvided()
        {
            _mockParser = new Mock<IGenericDataParser>();
            _service = new WorkflowStagingService(_mockParser.Object);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ThenFailIfSourceToolIsNullOrEmpty(string sourceTool)
        {
            var payload = JsonDocument.Parse("{\"name\":\"Test\"}").RootElement;

            var ex = Assert.Throws<ArgumentException>(() => _service.Stage(sourceTool, payload));
            Assert.Equal("Source tool is required.", ex.Message);
        }

        [Fact]
        public void ThenFailIfSourceToolIsUnknown()
        {
            var payload = JsonDocument.Parse("{\"name\":\"Test\"}").RootElement;
            var ex = Assert.Throws<ArgumentException>(() => _service.Stage("UnknownTool", payload));
            Assert.Contains("not recognized", ex.Message);
        }

        [Fact]
        public void ThenFailIfPayloadIsEmpty()
        {
            var payload = JsonDocument.Parse("{}").RootElement;
            var ex = Assert.Throws<ArgumentException>(() => _service.Stage("ToolA", payload));
            Assert.Equal("Payload is empty or invalid JSON.", ex.Message);
        }

        [Fact]
        public void ThenFailIfPayloadIsNotJsonObject()
        {
            var payload = JsonDocument.Parse("\"just a string\"").RootElement;
            var ex = Assert.Throws<ArgumentException>(() => _service.Stage("ToolA", payload));
            Assert.Equal("Payload is empty or invalid JSON.", ex.Message);
        }


        [Fact]
        public void ThenFailIfRespondentIsMissing()
        {
            var payload = JsonDocument.Parse("{\"other\":\"value\"}").RootElement;

            var ex = Assert.Throws<ArgumentException>(() => _service.Stage("ToolA", payload));
            Assert.Equal("Missing required field: 'respondent'.", ex.Message);
        }

        [Theory]
        [InlineData("{\"respondent\": \"\"}")]
        [InlineData("{\"respondent\": null}")]
        [InlineData("{\"respondent\": \"   \"}")]
        public void ThenFailIfRespondentIsEmpty(string json)
        {
            var payload = JsonDocument.Parse(json).RootElement;

            var ex = Assert.Throws<ArgumentException>(() => _service.Stage("ToolA", payload));
            Assert.Equal("Field 'respondent' must be a non-empty string.", ex.Message);
        }

        [Fact]
        public void ThenPassForValidPayload()
        {
            var mockParser = new Mock<IGenericDataParser>();

            var sourceTool = "ToolA";
            var payload = JsonDocument.Parse("{\"respondent\":\"Test\"}").RootElement;

            var expectedItem = new WorkflowItem
            {
                Id = Guid.NewGuid(),
                SourceTool = sourceTool,
                NormalizedData = new Dictionary<string, object> { { "respondent", "Test" } },
                Status = WorkflowStatus.PendingReview,
                ReceivedAt = DateTime.UtcNow
            };

            mockParser.Setup(p => p.Parse(sourceTool, payload)).Returns(expectedItem);

            var service = new WorkflowStagingService(mockParser.Object);

            // Act
            var result = service.Stage(sourceTool, payload);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sourceTool, result.SourceTool);
            Assert.Equal(WorkflowStatus.PendingReview, result.Status);
            Assert.True(result.NormalizedData.ContainsKey("respondent"));
            mockParser.Verify(p => p.Parse(sourceTool, payload), Times.Once);
        }
    }
}
