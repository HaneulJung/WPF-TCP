using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// ServerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ServerView : Window
    {
        private ChatServer _server;
        private ClientRoomManager _roomManager;

        private ChatHub CreateNewStateChatHub(ChatHub hub, ChatState state)
        {
            return new ChatHub
            {
                RoomId = hub.RoomId,
                UserName = hub.UserName,
                State = state
            };
        }

        private void AddClientMessageList(ChatHub hub)
        {
            string message = hub.State switch
            {
                ChatState.Connect => $"접속 {hub} ",
                ChatState.Disconnect => $"접속 종료 {hub}",
                _ => $"{hub} : {hub.Message}"
            };
            listBoxMessages.Items.Add(message);
        }

        private void Connected(object? sender, ChatEventArgs e)
        {
            var hub = CreateNewStateChatHub(e.ChatHub, ChatState.Connect);

            _roomManager.Add(e.ClientHandler);
            _roomManager.SendtoMyRoom(hub);

            listBoxClients.Items.Add(e.ChatHub);
            AddClientMessageList(hub);
        }

        private void Disconnected(object? sender, ChatEventArgs e)
        {
            var hub = CreateNewStateChatHub(e.ChatHub, ChatState.Disconnect);

            _roomManager.Remove(e.ClientHandler);
            _roomManager.SendtoMyRoom(hub);

            listBoxClients.Items.Remove(e.ChatHub);
            AddClientMessageList(hub);
        }

        private void Received(object? sender, ChatEventArgs e)
        {
            _roomManager.SendtoMyRoom(e.ChatHub);

            AddClientMessageList(e.ChatHub);
        }

        private void RunningStateChanged(bool isRunning)
        {
            btnStart.IsEnabled = !isRunning;
            btnEnd.IsEnabled = isRunning;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _ = _server.StartAsync();
        }
             
        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {
            _server.Stop();
        }

        public ServerView()
        {
            InitializeComponent();

            _roomManager = new ClientRoomManager();
            _server = new ChatServer(IPAddress.Parse("127.0.0.1"), 8080);
            _server.Connected += Connected;
            _server.Disconnected += Disconnected;
            _server.Received += Received;
            _server.RunningStateChanged += RunningStateChanged;
        }
    }
}
