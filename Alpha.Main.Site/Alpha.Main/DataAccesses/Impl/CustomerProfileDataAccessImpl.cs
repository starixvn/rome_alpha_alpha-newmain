namespace Alpha.Main.DataAccesses.Impl
{
    using Fanex.Common;
    using Fanex.Data;

    public class CustomerProfileDataAccessImpl : CustomerProfileDataAccess
    {
        public byte[] GetCustomerImageData(int custId)
        {
            byte[] imageData = new byte[] { };

            using (var db = new ObjectDb("Age_CustomerSelProfilePicture"))
            {
                var reader = db.ExecuteReader(new { custId = custId });

                if (reader.Read())
                {
                    imageData = Converter.ToByteArray(reader["image"]);
                }
            }

            return imageData;
        }
    }
}