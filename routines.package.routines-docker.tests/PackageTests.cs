using CliWrap.Exceptions;
using package.sdk;
using routines.package.docker;

namespace routines.package.docker_routines_new.tests;

public class PackageTests
{
    private readonly Entrypoint _entryPoint = new();
    private const string TEST_USERNAME = ""; //enter your username here
    private const string TEST_PASSWORD = ""; //enter your password here
    private const string TEST_REGISTRY = "docker.io";
    private const string TEST_IMAGE_NAME = "version-alert";
    private const string TEST_IMAGE_TAG = "latest";
    private const string TEST_CONTAINER_NAME = "test-version-alert-container";
    private const string TEST_COMMAND = "echo 'Hello from version-alert container'";
    private const string GRACE_PERIOD = "5";
    private const string LOG_TAIL_COUNT = "50";

    //Success Tests

    [Fact]
    public async Task Login_ValidCredentials_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string password = TEST_PASSWORD;
        var cancellationToken = CancellationToken.None;

        var result = await _entryPoint.Login(userName, registry, password, cancellationToken);
        

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    [Fact]
    public async Task PullImage_ValidImageName_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        var cancellationToken = CancellationToken.None;

        var result = await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    [Fact]
    public async Task CreateContainer_NoMappings_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        var cancellationToken = CancellationToken.None;

        try
        {
            
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            var result = await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task CreateContainer_PortMapping_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        var ports = new Dictionary<string, string> { { "80", "8080" } };
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            var result = await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, ports, null, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task StartContainer_PortsAndVolumesMapping_ReturnsSuccess()
    {
        // Arrange
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable = true;
        var ports = new Dictionary<string, string> { { "80", "8080" } };
        var volumes = new Dictionary<string, string> { { "C:/Users/DorRatzabi/Repos/Volume", "/app/myfolder" } };
        string containerName = $"{TEST_CONTAINER_NAME+Guid.NewGuid():N}";
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName,ports, volumes, cancellationToken);

            var result = await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.StopContainer(userName, registry, containerName, GRACE_PERIOD, cancellationToken);
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task GetContainerLogs_ReturnsSuccess()
    {
        // Arrange
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
            await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            var result = await _entryPoint.GetContainerLogs(userName, registry, containerName, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.StopContainer(userName, registry, containerName, GRACE_PERIOD, cancellationToken);
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task GetLogsWithTail_ReturnsSuccess()
    {
        // Arrange
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        string tail = LOG_TAIL_COUNT;
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
            await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            var result = await _entryPoint.GetLogsWithTail(userName, registry, containerName, tail, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.StopContainer(userName, registry, containerName, GRACE_PERIOD, cancellationToken);
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task ListRunningContainers_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        var cancellationToken = CancellationToken.None;

        var result = await _entryPoint.ListRunningContainers(userName, registry, cancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    [Fact]
    public async Task ListAllContainers_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        var cancellationToken = CancellationToken.None;

        var result = await _entryPoint.ListAllContainers(userName, registry, cancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    [Fact]
    public async Task ExecCommand_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        string command = TEST_COMMAND;
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
            await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            var result = await _entryPoint.ExecCommand(userName, registry, containerName, command, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.StopContainer(userName, registry, containerName, GRACE_PERIOD, cancellationToken);
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task StopContainer_ReturnsSuccess()
    {
        // Arrange
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        string gracePeriod = GRACE_PERIOD;
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
            await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            var result = await _entryPoint.StopContainer(userName, registry, containerName, gracePeriod, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task RemoveContainer_ReturnsSuccess()
    {
        // Arrange
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        var cancellationToken = CancellationToken.None;

        await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
        await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
        await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);
        await _entryPoint.StopContainer(userName, registry, containerName, GRACE_PERIOD, cancellationToken);

        var result = await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    [Fact]
    public async Task KillContainer_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        bool isRemovable= true;
        string containerName = $"{TEST_CONTAINER_NAME}_{Guid.NewGuid():N}";
        var cancellationToken = CancellationToken.None;

        try
        {
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken);
            await _entryPoint.CreateContainer(userName, registry, imageName, tag, isRemovable, containerName, null, null, cancellationToken);
            await _entryPoint.StartContainer(userName, registry, containerName, cancellationToken);

            
            var result = await _entryPoint.KillContainer(userName, registry, containerName, cancellationToken);

            Assert.NotNull(result);
            Assert.NotNull(result.Output);
            Assert.True(result.Status == Status.Success);
        }
        finally
        {
            await _entryPoint.RemoveContainer(userName, registry, containerName, cancellationToken);
        }
    }

    [Fact]
    public async Task LogOut_ReturnsSuccess()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        var cancellationToken = CancellationToken.None;

        var result = await _entryPoint.LogOut(userName, registry, cancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Output);
        Assert.True(result.Status == Status.Success);
    }

    //Failures Tests

    [Fact] 
    public async Task Login_InvalidCredentials_ThrowsCommandExecutionException()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        var cancellationToken = CancellationToken.None;
        string password = "dummy_string";

        await Assert.ThrowsAsync<CommandExecutionException>(async () =>
    await _entryPoint.Login(userName, registry, password, cancellationToken)
          
    );

    }


    [Fact]
    public async Task PullImage_InvalidImageName_ThrowsCommandExecutionException()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = "dummy_not_exist_image";
        string tag = TEST_IMAGE_TAG;
        var cancellationToken = CancellationToken.None;

        await Assert.ThrowsAsync<CommandExecutionException>(async () =>
        await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationToken)
        );
   

    }

    [Fact]
    public async Task Login_CancellationTokenUsage_ThrowsOperationCanceledException()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string password = TEST_PASSWORD;
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await _entryPoint.Login(userName, registry, password, cancellationTokenSource.Token));
    }

    [Fact]
    public async Task PullImage_CancellationTokenUsage_ThrowsOperationCanceledException()
    {
        string userName = TEST_USERNAME;
        string registry = TEST_REGISTRY;
        string imageName = TEST_IMAGE_NAME;
        string tag = TEST_IMAGE_TAG;
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await _entryPoint.PullImage(userName, registry, imageName, tag, cancellationTokenSource.Token));
    }
}