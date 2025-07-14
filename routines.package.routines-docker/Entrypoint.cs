using package.sdk;
using CliWrap.Buffered;
using routines.package.docker.DockerOps;

namespace routines.package.docker;

public class Entrypoint : PackageBase
{


    public async Task<MethodOutput> Login(string userName, string registry, string password, CancellationToken cancellationToken= default)
    {
        
            DockerHandler dockerHandler = new (userName, registry);
            BufferedCommandResult login = await dockerHandler.Login(password, cancellationToken);
            Log(login.StandardOutput.Trim());

            return new MethodOutput()
            {
                Output = login.StandardOutput.Trim(),
                Status = login.ExitCode == 0 ? Status.Success : Status.Failure,
                StatusCode= login.ExitCode==0? 200 : 500,

            };   
    }

    public async Task<MethodOutput> PullImage(string userName, string registry, string imageName, string tag, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult pull = await dockerHandler.PullImage(imageName, tag, cancellationToken);
        return new MethodOutput()
        {
            Output = pull.StandardOutput.Trim(),
            Status = pull.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = pull.ExitCode == 0 ? 200 : 500,
        };
    }

    public async Task<MethodOutput> CreateContainer(string userName, string registry, string imageName, string tag, bool isRemovable, string containerName, Dictionary<string, string>? ports = null, Dictionary<string, string>? volumes = null, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult create = await dockerHandler.CreateContainer(imageName, tag, containerName, isRemovable, ports, volumes, cancellationToken);
        return new MethodOutput()
        {
            Output = create.StandardOutput.Trim(),
            Status = create.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = create.ExitCode == 0 ? 200 : 500,
        };
    }

    public async Task<MethodOutput> StartContainer(string userName, string registry, string containerName, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult start = await dockerHandler.StartContainer(containerName, cancellationToken);
        return new MethodOutput()
        {
            Output = start.StandardOutput.Trim(),
            Status = start.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = start.ExitCode == 0 ? 200 : 500,
        };
    }

    public async Task<MethodOutput> GetContainerLogs(string userName, string registry, string containerName, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult logs = await dockerHandler.GetContainerLogs(containerName,null, cancellationToken);
        return new MethodOutput()
        {
            Output = logs.StandardOutput.Trim(),
            Status = logs.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = logs.ExitCode == 0 ? 200 : 500,
        };

    }

    public async Task<MethodOutput> GetLogsWithTail(string userName, string registry, string containerName, string tail = "10", CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult logs = await dockerHandler.GetContainerLogs(containerName, tail, cancellationToken);
        return new MethodOutput()
        {
            Output = logs.StandardOutput.Trim(),
            Status = logs.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = logs.ExitCode == 0 ? 200 : 500,
        };
    }

    public async Task<MethodOutput> ListRunningContainers(string userName, string registry, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult list = await dockerHandler.ListContainers(false,cancellationToken);
        return new MethodOutput()
        {
            Output = list.StandardOutput.Trim(),
            Status = list.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = list.ExitCode == 0 ? 200 : 500,
        };
    }

    public async Task<MethodOutput> ListAllContainers(string userName, string registry, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult list = await dockerHandler.ListContainers(true, cancellationToken);
        return new MethodOutput()
        {
            Output = list.StandardOutput.Trim(),
            Status = list.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = list.ExitCode == 0 ? 200 : 500,

        };
    }

    public async Task<MethodOutput> ExecCommand(string userName, string registry, string containerName, string command, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult exec = await dockerHandler.RunCommandOnContainer(containerName, command, cancellationToken);
        return new MethodOutput()
        {
            Output = exec.StandardOutput.Trim(),
            Status = exec.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = exec.ExitCode == 0 ? 200 : 500,
        };

    }

    public async Task<MethodOutput> StopContainer(string userName, string registry, string containerName, string gracePeriod = "10", CancellationToken cancellationToken = default)
    {

        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult stop = await dockerHandler.StopContainer(containerName, gracePeriod, cancellationToken);


        return new MethodOutput()
        {
            Output = stop.StandardOutput.Trim(),
            Status = stop.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = stop.ExitCode == 0 ? 200 : 500,
        };

    }

    public async Task<MethodOutput> RemoveContainer(string userName, string registry, string containerName, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult remove = await dockerHandler.RemoveContainer(containerName, true, cancellationToken);
        return new MethodOutput()
        {
            Output = remove.StandardOutput.Trim(),
            Status = remove.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = remove.ExitCode == 0 ? 200 : 500,
        };
    }


    public async Task<MethodOutput> KillContainer(string userName, string registry, string containerId, CancellationToken cancellationToken = default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult kill = await dockerHandler.KillContainer(containerId, cancellationToken);
        return new MethodOutput()
        {
            Output = kill.StandardOutput.Trim(),
            Status = kill.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = kill.ExitCode == 0 ? 200 : 500,
        };
    }
    


    public async Task<MethodOutput> LogOut(string userName, string registry, CancellationToken cancellationToken= default)
    {
        DockerHandler dockerHandler = new DockerHandler(userName, registry);
        BufferedCommandResult logout = await dockerHandler.LogOut(cancellationToken);
        return new MethodOutput()
        {
            Output = logout.StandardOutput.Trim(),
            Status = logout.ExitCode == 0 ? Status.Success : Status.Failure,
            StatusCode = logout.ExitCode == 0 ? 200 : 500,
        };

    }
}