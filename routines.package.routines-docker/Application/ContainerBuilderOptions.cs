

namespace routines.package.docker.Application;

public static class OptionFilesBuilder
{


    public static List<string> BuildPortsBindings(Dictionary<string, string> ports, List<string> arguments)
    {
        foreach (var port in ports)
        {
            arguments.Add("-p");
            arguments.Add($"{port.Key}:{port.Value}");
        }
        return arguments;
    }
    public static List<string> AttachVolumes(Dictionary<string, string> volumes, List<string> arguments)
    {
        foreach (var volume in volumes)
        {
            arguments.Add("-v");
            var hostPath = volume.Key.Replace("\\", "/");
            arguments.Add($"{hostPath}:{volume.Value}");
        }
        return arguments;
    }
}
