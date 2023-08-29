using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TCPPrac.Models
{
    public class ChatHub : ConnectionDetails
    {
        public static ChatHub? Parse(string json) => JsonSerializer.Deserialize<ChatHub>(json);

        public ChatState State { get; set; }
        public string Message { get; set; } = string.Empty;

        public string ToJsonString() => JsonSerializer.Serialize(this);
    }
}
