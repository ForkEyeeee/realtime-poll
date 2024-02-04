//using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
//using realTimePolls.Models;

//namespace RazorPagesProject.Tests.Helpers;

//public static class Utilities
//{
//    // <snippet1>
//    public static void InitializeDbForTests(RealTimePollsContext db)
//    {
//        db.Messages.AddRange(GetSeedingMessages());
//        db.SaveChanges();
//    }

//    public static void ReinitializeDbForTests(RealTimePollsContext db)
//    {
//        db.Messages.RemoveRange(db.Messages);
//        InitializeDbForTests(db);
//    }

//    public static List<Message> GetSeedingMessages()
//    {
//        return new List<Message>()
//        {
//            new Message() { Text = "TEST RECORD: You're standing on my scarf." },
//            new Message() { Text = "TEST RECORD: Would you like a jelly baby?" },
//            new Message()
//            {
//                Text =
//                    "TEST RECORD: To the rational mind, "
//                    + "nothing is inexplicable; only unexplained."
//            }
//        };
//    }
//    // </snippet1>
//}
