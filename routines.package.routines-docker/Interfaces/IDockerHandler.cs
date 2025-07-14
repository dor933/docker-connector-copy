using CliWrap.Buffered;


namespace routines.package.Docker.Interfaces;

internal interface IDockerHandler
{
    Task<BufferedCommandResult> Login(string password, CancellationToken cancellationToken);

    Task<BufferedCommandResult> PullImage(string imageName, string tag, CancellationToken cancellationToken);

    Task<BufferedCommandResult> PushImage(string imageName, string tag, CancellationToken cancellationToken);

    Task<BufferedCommandResult> CreateContainer(string imageName, string imageTag, string containerName, bool isRemovable = false, Dictionary<string, string>? ports = null, Dictionary<string, string>? volumes = null, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> StartContainer(string containerId, CancellationToken cancellationToken);

    Task<BufferedCommandResult> StopContainer(string containerId, string gracePeriod = "10", CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> KillContainer(string containerId, CancellationToken cancellationToken);

    Task<BufferedCommandResult> RemoveContainer(string containerId, bool forcefully = false, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> ListContainers(bool all = false, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> RunCommandOnContainer(string containerId, string command, CancellationToken cancellationToken);

    Task<BufferedCommandResult> GetContainerLogs(string containerId, string? lines = null, CancellationToken cancellationToken = default);

    Task<BufferedCommandResult> LogOut(CancellationToken cancellationToken);

}
