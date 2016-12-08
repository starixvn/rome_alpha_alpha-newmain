namespace Alpha.Main.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Models.Message;

    public interface MessageDataAccess
    {
        List<Message> GetMessages(ICriteria criteria, int messageType);
    }
}