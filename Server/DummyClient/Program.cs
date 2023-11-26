using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            // 로컬 컴퓨터의 ip를 가져와 도메인으로 변환한 뒤 변환 주소를 ipAdd에 저장
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAdd = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAdd, 7777);  // IpAddress와 포트 넘버를 인자로 넘겨줌
            
            // 소켓 설정
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 서버 소켓에 connect 문의
                socket.Connect(endPoint);
                // 연결된 서버의 EndPoint를 출력
                Console.WriteLine($"connected To {socket.RemoteEndPoint.ToString()}");
            
                // 정보를 보낸다
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World !");
                int sendBytes = socket.Send(sendBuff);

                // 정보를 받는다
                byte[] recvBuff = new byte[1024];
                int byteCount = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, byteCount);
                Console.WriteLine($"[From Server] {recvData}");
            
                // 나간다
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}