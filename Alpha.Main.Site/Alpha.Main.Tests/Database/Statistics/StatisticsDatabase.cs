namespace Alpha.Main.Tests.Database.Statistics
{
    using System.Data;
    using Helps;

    public static class StatisticsDatabase
    {
        private static DataSet statisticsData = FileHelper.LoadDataSet(@"\DataBase\Statistics\StatisticsHasData.xml");

        public static DataSet StatisticsData => statisticsData;
    }
}
