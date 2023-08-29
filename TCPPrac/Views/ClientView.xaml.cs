using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TCPPrac.Events;
using TCPPrac.Handlers;
using TCPPrac.Models;
using TCPPrac.Sockets;

namespace TCPPrac.Views
{
    /// <summary>
    /// ClientView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ClientView : Window
    {

        private ChatClient _client;
        private ClientHandler? _clientHandler;

        private int RoomId => int.Parse(txtId.Text);
        private string UserName => txtName.Text;
        private string Message => txtMessage.Text;


        private void RunningStateChanged(bool isRunning)
        {
            btnConnect.IsEnabled = !isRunning;
            btnDisconnect.IsEnabled = isRunning;
        }

        private void Connected(object? sender, ChatEventArgs e)
        {
            _clientHandler = e.ClientHandler;
        }

        private void Disconnected(object? sender, ChatEventArgs e)
        {
            _clientHandler = null;
            ListBoxClients.Items.Add("서버와의 연결이 끊겼습니다.");
        }

        private void Received(object? sender, ChatEventArgs e)
        {
            ChatHub hub = e.ChatHub;
            string message = hub.State switch
            {
                ChatState.Connect => $"{hub.UserName}님이 접속하였습니다.",
                ChatState.Disconnect => $"{hub.UserName}님이 종료하였습니다.",
                _ => $"{hub.UserName}: {hub.Message}"
            };

            ListBoxClients.Items.Add(message);
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            await _client.ConnectAsync(new ConnectionDetails
            {
                RoomId = RoomId,
                UserName = UserName,
            });
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            _client.Close();
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key== Key.Return)
            {
                _clientHandler?.Send(new ChatHub
                {
                    RoomId = RoomId,
                    UserName = UserName,
                    Message = Message
                });
                txtMessage.Clear();
            }
        }

        public ClientView()
        {
            InitializeComponent();

            _client = new ChatClient(IPAddress.Parse("127.0.0.1"), 8080);
            _client.Connected += Connected;
            _client.Disconnected += Disconnected;
            _client.Received += Received;
            _client.RunningStateChanged += RunningStateChanged;
        }
    }
}
