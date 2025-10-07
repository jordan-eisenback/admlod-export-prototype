using Xunit;

public class MockResponseGeneratorTests
{
    [Fact]
    public void ShouldReturnSuccessForMergeLines()
    {
        var generator = new MockResponseGenerator();
        var input = "MERGE;PRODUCT;D01;Diesel #1;D";

        var result = generator.GenerateResponse(input);

        Assert.Contains("Success: MERGE;PRODUCT;D01;Diesel #1;D", result);
    }

    [Fact]
    public void ShouldReturnErrorForErrorLines()
    {
        var generator = new MockResponseGenerator();
        var input = "# ERROR: Missing Product Code";

        var result = generator.GenerateResponse(input);

        Assert.Contains("Error: Missing Product Code", result);
    }
}