using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using Firebase.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using FirebaseAdmin.Messaging;

namespace BLL.Service
{
    public class ChatService
    {
        private FirestoreDb db;
        public ChatService()
        {
            string path = @"firebase.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            db = FirestoreDb.Create("pushland-ed3b1");
        }
        
        public async Task SendMessage(ChatMessage message)
        {
            // Store the message in the Firebase Realtime Database
            var data = new Dictionary<string, object>()
            {
                { "to", message.To },
                { "message", message.Message },
                { "timestamp", DateTime.UtcNow }
            };
            await db.Collection("chats").AddAsync(data);
        }
        
    }
    
    public class ChatMessage
    {
        public int To { get; set; }
        public string Message { get; set; }
    }
}
