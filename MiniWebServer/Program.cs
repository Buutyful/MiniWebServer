using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new TcpListener(IPAddress.Any, 6969);
server.Start();

while (true)
{
    var client = server.AcceptTcpClient();
    _ = HandleClient(client);
}

static async Task HandleClient(TcpClient client)
{
    try
    {
        using var data = client.GetStream();
        var buffer = new byte[1024];
        var bytesRead = await data.ReadAsync(buffer);

        var request = Encoding.UTF8.GetString(buffer);

        Console.WriteLine(request);

        var response =
            "HTTP/1.1 200 OK\r\n" +
            "Content-Type: text/plain\r\n" +
            "Content-Length: " + bytesRead + "\r\n" +
            "\r\n" + request;

        var message = Encoding.UTF8.GetBytes(response);

        await data.WriteAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        client.Close();
    }
}

