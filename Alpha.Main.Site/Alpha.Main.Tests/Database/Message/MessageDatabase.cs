namespace Alpha.Main.Tests.Database.Message
{
    using System.Data;
    using Helps;

    public static class MessageDatabase
    {
        private static DataSet messageData = FileHelper.LoadDataSet(@"\DataBase\Message\MessageHasData.xml");

        public static DataSet MessageData => messageData;
    }
}
