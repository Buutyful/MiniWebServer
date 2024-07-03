using System.Net;
using System.Net.Sockets;
using System.Text;

var server = new TcpListener(IPAddress.Any, 6969);
server.Start();

while (true)
{
    var client = server.AcceptTcpClient();
    var data = client.GetStream();

    var buffer = new byte[1024];
    var bytesRead = data.Read(buffer, 0, buffer.Length);

    var request = Encoding.UTF8.GetString(buffer);

    Console.WriteLine(request);
    
    var response =
        "HTTP/1.1 200 OK\r\n" +
        "Content-Type: text/plain\r\n" +
        "Content-Length: " + bytesRead + "\r\n" +
        "\r\n" + request;

    var message = Encoding.UTF8.GetBytes(response);

    data.Write(message);

    client.Close();
}

