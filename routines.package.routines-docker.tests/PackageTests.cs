using package.sdk;

namespace routines.package.routines_docker.tests;

public class PackageTests
{
    private readonly Entrypoint _entrypoint = new();
    
    [Fact]
    public void SayHello_ReturnsCorrectOutput()
    {
        var result = _entrypoint.SayHello("World");

        Assert.Equal("Hello, World!", result.Output);
        Assert.Equal(Status.Success, result.Status);
        Assert.Equal(0, result.StatusCode);
    }

    [Fact]
    public async Task SumNumbers_ReturnsCorrectSum()
    {
        var result = await _entrypoint.SumNumbers(3, 4);

        Assert.Equal("The sum of 3 and 4 is 7.", result.Output);
        Assert.Equal(Status.Success, result.Status);
        Assert.Equal(0, result.StatusCode);
    }

    [Fact]
    public async Task SumNumbers_UsesDefaultValueForSecondParameter()
    {
        var result = await _entrypoint.SumNumbers(5);

        Assert.Equal("The sum of 5 and 1 is 6.", result.Output);
        Assert.Equal(Status.Success, result.Status);
        Assert.Equal(0, result.StatusCode);
    }

    [Fact]
    public async Task SumNumbers_CancellationRequested_ThrowsTaskCanceledException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            await _entrypoint.SumNumbers(1, 2, cancellationTokenSource.Token));
    }
}