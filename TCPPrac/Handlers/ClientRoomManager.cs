using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPPrac.Models;
using TCPPrac.Sockets;

namespace TCPPrac.Handlers
{
    public class ClientRoomManager
    {
        private Dictionary<int, List<ClientHandler>> _roomHandlerDict = new();

        public void Add(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlerDict.TryGetValue(roomId, out _))
            {
                _roomHandlerDict[roomId].Add(clientHandler);
            }
            else
            {
                _roomHandlerDict[roomId] = new List<ClientHandler>() { clientHandler };
            }
        }

        public void Remove(ClientHandler clientHandler)
        {
            int roomId = clientHandler.InitialData!.RoomId;

            if (_roomHandlerDict.TryGetValue(roomId, out List<ClientHandler>? roomHandlers))
            {
                _roomHandlerDict[roomId] = roomHandlers.FindAll(handler => !handler.Equals(clientHandler));
            }
        }

        public void SendtoMyRoom(ChatHub hub)
        {
            if (_roomHandlerDict.TryGetValue(hub.RoomId, out List<ClientHandler>? roomHandlers))
            {
                roomHandlers.ForEach(handler => handler.Send(hub));
            }
        }
    }
}
