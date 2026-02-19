using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace ToastrForge.Services;

public static class SingleInstanceService
{
    private const string PipeName = "com.toastrforge.tasbeeh.pipe";

    public static void StartServer(Action onSignalReceived)
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    using var server = new NamedPipeServerStream(PipeName, PipeDirection.In);
                    server.WaitForConnection();

                    using var reader = new StreamReader(server);
                    var message = reader.ReadToEnd();

                    if (message == "SHOW")
                    {
                        onSignalReceived?.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Pipe Server Error: {ex.Message}");
                    Thread.Sleep(1000);
                }
            }
        });
    }

    public static void NotifyExistingInstance()
    {
        try
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out);
            client.Connect(1000);

            using var writer = new StreamWriter(client);
            writer.Write("SHOW");
            writer.Flush();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Pipe Client Error: {ex.Message}");
        }
    }
}
