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
        private FirebaseAuthProvider auth;
        private FirestoreDb db;
        private FirebaseAuthLink authLink;
        public async Task Connect()
        {
            db = FirestoreDb.Create("");
            var config = new FirebaseConfig("<firebase-api-key>");


            // Create a new instance of FirebaseAuthProvider with the Firebase configuration
            this.auth = new FirebaseAuthProvider(config);

            authLink = await auth.SignInWithEmailAndPasswordAsync("<email>", "<password>");
        }
        public async Task SendMessage(string message)
        {
            // Store the message in the Firebase Realtime Database
            var data = new Dictionary<string, object>()
            {
                { "user", authLink.User.Email },
                { "message", "<message-text>" },
                { "timestamp", DateTime.UtcNow }
            };
            await db.Collection("chats").Document().SetAsync(data);
        }
        public Task StartListeningAsync(Action<ChatMessage> callback)
        {
            db.Collection("chats")
            .OrderBy("timestamp")
            .Listen(async snapshot =>
            {
                foreach (var documentChange in snapshot.Changes)
                {
                    if (documentChange.ChangeType == DocumentChange.Type.Added)
                    {
                        var chatMessage = new ChatMessage
                        {
                            User = documentChange.Document.GetValue<string>("user"),
                            Message = documentChange.Document.GetValue<string>("message"),
                            Timestamp = documentChange.Document.GetValue<DateTime>("timestamp")
                        };
                        callback(chatMessage);
                    }
                }
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = "New message",
                        Body = "You have a new message"
                    },
                    Token = "device_registration_token"
                };
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            });
            return Task.CompletedTask;
        }
    }
    
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
