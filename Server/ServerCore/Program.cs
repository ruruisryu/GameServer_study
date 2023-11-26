using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
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
            
            // Listner(문지기) 생성
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            
            // Bind -> 문지기 교육
            // 서버의 Ip주소와 포트 넘버 정보를 가지고 있는 EndPoint를 넣어줌
            listenSocket.Bind(endPoint);
            
            // Listen -> 영업 시작
            // 인자 backLog: 최대 대기수
            listenSocket.Listen(10);

            try
            {
                while (true)
                {
                    Console.WriteLine("Listening...");
                
                    // Accept -> 손님을 입장시킨다.
                    // Accept는 클라이언트의 소켓을 반환
                    Socket clientSocket = listenSocket.Accept();
                
                    // 정보를 받는다
                    byte[] recvBuff = new byte[1024];                   // 정보를 받을 버퍼
                    int recvBytes = clientSocket.Receive(recvBuff);     // 받은 정보의 바이트수
                    // 받은 정보(바이트)를 string으로 인코딩
                    // 인자 세가지는 인코딩할 바이트 배열, 시작 인덱스, 바이트 개수
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); 
                    Console.WriteLine($"[From Client]: {recvData}");

                    // 정보를 보낸다
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG server !");
                    clientSocket.Send(sendBuff);
                
                    // 쫓아낸다
                    // clientSocket.Shutdown(SocketShutdown.Both);  // 우아하게 예고
                    clientSocket.Close();   // 쫓아내기
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
