using CliWrap;
using routines.package.docker.Application;
using CliWrap.Buffered;
using routines.package.Docker.Interfaces;


namespace routines.package.docker.DockerOps;



public class DockerHandler : IDockerHandler
{
    private readonly string _userName;
    private readonly string _registry;
    private const string DOCKER_UTILITY = "docker";



    public DockerHandler(string userName, string registry)
    {
        this._userName = userName;
        this._registry = registry;

    }



    public async Task<BufferedCommandResult> Login(string password, CancellationToken cancellationToken)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments( ["login", _registry, "-u", _userName, "-p", password])
        .ExecuteBufferedAsync(cancellationToken);

        return result;

    }


    public async Task<BufferedCommandResult> PullImage(string imageName, string tag, CancellationToken cancellationToken)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments([ "pull", $"{_registry}/{_userName}/{imageName}:{tag}" ])
        .ExecuteBufferedAsync(cancellationToken);

        return result;

    }

    public async Task<BufferedCommandResult> PushImage(string imageName, string tag, CancellationToken cancellationToken)
    {
        await Cli.Wrap(DOCKER_UTILITY).WithArguments( [ "tag", $"{imageName + ":" + tag}", $"{_userName + "/" + imageName + ":" + tag}" ]).ExecuteBufferedAsync(cancellationToken);

        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments( ["push", $"{_registry}/{_userName}/{imageName}:{tag}" ])
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> CreateContainer(string imageName, string imageTag, string containerName, string isRemovable = "false", Dictionary<string, string>? ports = null, Dictionary<string, string>? volumes = null, CancellationToken cancellationToken=default)
    {
        List<string> arguments = new List<string> { "create" };

        if (isRemovable.ToLower() == "true")
        {
            arguments.Add("--rm");
        }

        arguments.Add("--name");
        arguments.Add(containerName);

        if (ports?.Any()==true)
            OptionFilesBuilder.BuildPortsBindings(ports, arguments);


        if (volumes?.Any()==true)
            OptionFilesBuilder.AttachVolumes(volumes, arguments);

        arguments.Add($"docker.io/dor93/{imageName}:{imageTag}");

        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
            .WithArguments(args => args.Add(arguments))
            .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> StartContainer(string containerId, CancellationToken cancellationToken)
    {

        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments(["start", containerId])
        .ExecuteBufferedAsync();

        return result;
    }


    public async Task<BufferedCommandResult> StopContainer(string containerId, string gracePeriod = "10", CancellationToken cancellationToken = default)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments(["stop", "--time=" + gracePeriod, containerId])
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> KillContainer(string containerId, CancellationToken cancellationToken)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments([ "kill", containerId ])
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> RemoveContainer(string containerId, bool forcefully = false, CancellationToken cancellationToken = default)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments([ "rm", forcefully ? "-f" : "", containerId ])
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> ListContainers(bool all = false, CancellationToken cancellationToken = default)
    {
        List<string> arguments = new List<string> { "ps" };
        if (all)
            arguments.Add("-a");

        arguments.Add("--format");
        arguments.Add("{{json .}}");

        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments(args => args.Add(arguments))
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> RunCommandOnContainer(string containerId, string command, CancellationToken cancellationToken)
    {
        List<string> commandParts= command.Split(' ').ToList();
        commandParts.Insert(0, "exec");
        commandParts.Insert(1, containerId);
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
            .WithArguments(args => args.Add(commandParts))
            .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> GetContainerLogs(string containerId, string? lines = null, CancellationToken cancellationToken = default)
    {
        List<string> arguments = new List<string> { "logs", containerId };
        if (!String.IsNullOrEmpty(lines))
        {
            arguments.Add("--tail");
            arguments.Add(lines);
        }

        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments(args => args.Add(arguments))
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

    public async Task<BufferedCommandResult> LogOut(CancellationToken cancellationToken)
    {
        BufferedCommandResult result = await Cli.Wrap(DOCKER_UTILITY)
        .WithArguments([ "logout", _registry ])
        .ExecuteBufferedAsync(cancellationToken);

        return result;
    }

}

