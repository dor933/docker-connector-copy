using package.sdk;

namespace routines.package.routines_docker;

public class Entrypoint : PackageBase
{
    public MethodOutput SayHello(string name)
    {
        Log("SayHello called.");
        
        return new MethodOutput
        {
            Output = $"Hello, {name}!",
            Status = Status.Success,
            StatusCode = 0
        };
    }

    public async Task<MethodOutput> SumNumbers(int number1, int number2 = 1, CancellationToken cancellationToken = default)
    {
        Log("SumNumbers called.");
        
        await Task.Delay(1000, cancellationToken);

        return new MethodOutput
        {
            Output = $"The sum of {number1} and {number2} is {number1 + number2}.",
            Status = Status.Success,
            StatusCode = 0
        };
    }
}