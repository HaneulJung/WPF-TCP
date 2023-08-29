using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPPrac.Handlers;
using TCPPrac.Models;

namespace TCPPrac.Events
{
    public class ChatEventArgs : EventArgs
    {
        public ClientHandler ClientHandler { get; }
        public ChatHub ChatHub { get; }

        public ChatEventArgs(ClientHandler clientHandler, ChatHub chatHub) 
        {
            ClientHandler = clientHandler;
            ChatHub = chatHub;
        }
    }
}
