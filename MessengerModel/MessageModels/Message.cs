﻿using MessengerModel.ChatModelds;
using MessengerModel.UserModels;

namespace MessengerModel.MessageModels
{
    public class Message
    {
        public DateTime Date { get; set; }
        public Guid Guid { get; set; }
        public Chat Chat { get; set; } = null!;
        public Guid ChatGuid { get; set; }
        public User User { get; set; } = null!;
        public Guid UserGuid { get; set; }
        public string Text { get; set; } = string.Empty;
        public ICollection<MessageComment> MessageComment { get; set; } = new List<MessageComment>();
    }
}