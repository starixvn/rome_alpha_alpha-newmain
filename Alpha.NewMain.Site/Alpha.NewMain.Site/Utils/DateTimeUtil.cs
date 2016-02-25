namespace Alpha.NewMain.Site.Utils
{
    using System;
    using Fanex.Common;
    using Sunplus.Agent.Model.Entities;

    public class DateTimeUtil
    {
        public static DateTime ConvertToDateTimeFromShortDateString(string dateTime)
        {
            switch (UserSession.Current.Language)
            {
                case "vi-VN":
                    var dateTimeParts = dateTime.Split('/');
                    return new DateTime(Converter.ToInt(dateTimeParts[2]), Converter.ToInt(dateTimeParts[0]), Converter.ToInt(dateTimeParts[1]));
                default:
                    return Convert.ToDateTime(dateTime);
            }
        }
    }
}