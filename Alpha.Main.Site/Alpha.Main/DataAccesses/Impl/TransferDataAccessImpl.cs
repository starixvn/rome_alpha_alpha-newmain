namespace Alpha.Main.DataAccesses.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class TransferDataAccessImpl : TransferDataAccess
    {
        public bool IsAllDayBalanceTransfer(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    return reader.Read() && Convert.ToBoolean(reader["IsAllDayBalTransfer"]);
                }
            }
        }
    }
}