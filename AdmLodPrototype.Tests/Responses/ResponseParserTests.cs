using Xunit;

public class ResponseParserTests
{
    [Fact]
    public void ShouldCountSuccessAndErrorLines()
    {
        var input = @"
Success: MERGE;PRODUCT;D01;Diesel #1;D
Error: Missing Product Name
Success: MERGE;PGRP;DIESEL";

        var result = ResponseParser.ParseResponse(input);

        Assert.Equal(2, result.SuccessCount);
        Assert.Equal(1, result.ErrorCount);
    }
}
