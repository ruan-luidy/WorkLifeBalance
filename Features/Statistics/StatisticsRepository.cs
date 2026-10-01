using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkLifeBalance.Shared.Data;

namespace WorkLifeBalance.Features.Statistics
{
    public class StatisticsRepository
    {
        private readonly SqlDataAccess dataAccess;

        public StatisticsRepository(SqlDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<List<ProcessActivityData>> ReadProcessDayActivity(string date)
        {
            List<ProcessActivityData> processActivity = new();

            string sql = @$"SELECT Date, Process, TimeSpent from Activity 
                            WHERE Date Like @Date AND Process IS NOT NULL";

            processActivity = (await dataAccess.ReadDataAsync<ProcessActivityData, dynamic>(sql, new { Date = date })).ToList();

            foreach (ProcessActivityData day in processActivity)
            {
                day.ConvertSaveDataToUsableData();
            }

            return processActivity;
        }

        
        public async Task<List<PageActivityData>> ReadUrlDayActivity(string date)
        {
            List<PageActivityData> pageActivity = new();

            string sql = @$"SELECT Date, Url, TimeSpent from Activity 
                            WHERE Date Like @Date AND Url IS NOT NULL";

            pageActivity = (await dataAccess.ReadDataAsync<PageActivityData, dynamic>(sql, new { Date = date })).ToList();

            foreach (PageActivityData day in pageActivity)
            {
                day.ConvertSaveDataToUsableData();
            }

            return pageActivity;
        }

        public async Task<int> ReadCountInMonth(string month, string year)
        {
            int affectedRows = 0;
            string sql = @$"SELECT COUNT(*) AS row_count
                            FROM Days WHERE date LIKE @Pattern";
            affectedRows = await dataAccess.ExecuteAsync(sql, new { Pattern = $"{month}%{year}" });

            return affectedRows;
        }

        public async Task<List<DayData>> ReadMonth(string Month = "", string year = "")
        {
            List<DayData> ReturnDays = new();

            string sql;
            if (string.IsNullOrEmpty(Month) || string.IsNullOrWhiteSpace(year))
            {
                sql = @$"SELECT * from Days";
            }
            else
            {
                sql = @$"SELECT * from Days 
                        WHERE Date Like @Pattern";
            }

            ReturnDays = (await dataAccess.ReadDataAsync<DayData, dynamic>(sql, new { Pattern = $"{Month}%{year}" })).ToList();

            foreach (DayData day in ReturnDays)
            {
                day.ConvertSaveDataToUsableData();
            }

            return ReturnDays;
        }

        public async Task<DayData> GetMaxValue(string collumnData, string Month = "", string year = "")
        {
            DayData? retrivedDay = null;

            string sql;

            if (string.IsNullOrEmpty(Month) || string.IsNullOrEmpty(year))
            {
                //pass the value directly because it brokes if I use it as a parameter
                sql = @$"SELECT * FROM Days 
                      WHERE CAST({collumnData} as INT) = 
                      (SELECT MAX(CAST({collumnData} as INT)) FROM Days)";
                retrivedDay = (await dataAccess.ReadDataAsync<DayData, dynamic>(sql, new { })).FirstOrDefault();
            }
            else
            {
                sql = @$"SELECT * FROM Days 
                        WHERE CAST({collumnData} as INT) = 
                        (SELECT MAX(CAST({collumnData} as INT)) FROM Days
                        WHERE Date LIKE @Template)";


                retrivedDay = (await dataAccess.ReadDataAsync<DayData, dynamic>(sql, new { Template = $"{Month}%{year}" })).FirstOrDefault();
            }

            retrivedDay ??= new();

            retrivedDay.ConvertSaveDataToUsableData();

            return retrivedDay;
        }

        public async Task<int> GetAvgSecondsTimeOnly(string timeOnlyCollumn, string Month = "", string year = "")
        {
            int avgAmount;

            string sql = @$"WITH ConvertedTimes AS 
                    (
                        SELECT 
                        (CAST(SUBSTR({timeOnlyCollumn}, 1, 2) AS INTEGER) * 3600 +
                        CAST(SUBSTR({timeOnlyCollumn}, 3, 2) AS INTEGER) * 60 +
                        CAST(SUBSTR({timeOnlyCollumn}, 5, 2) AS INTEGER)) AS TotalSeconds, date FROM Days
                    )
                    SELECT COALESCE(AVG(TotalSeconds), 0) AS AvgSeconds
                    FROM ConvertedTimes WHERE date LIKE @Template";

            avgAmount = (await dataAccess.ReadDataAsync<int, dynamic>(sql, new { Template = $"{Month}%{year}" })).FirstOrDefault();

            return avgAmount;
        }

        public async Task<ProcessActivityData> GetMostActiveActivity(string activity, string Month = "", string year = "")
        {
            ProcessActivityData? retrivedDay = null;

            string sql;
            if (string.IsNullOrEmpty(Month) || string.IsNullOrEmpty(year))
            {
                sql = @$"SELECT * FROM Days 
                        WHERE @Activity = 
                        (SELECT MAX(@Activity) FROM Days)";
            }
            else
            {

                sql = @$"SELECT * FROM Days 
                        WHERE @Activity = 
                        (SELECT MAX(@Activity) FROM Days
                        WHERE Date Like @Pattern)";
            }
            retrivedDay = (await dataAccess.ReadDataAsync<ProcessActivityData, dynamic>(sql, new { Activity = activity, Pattern = $"{Month}%{year}" })).FirstOrDefault();

            retrivedDay ??= new();

            retrivedDay.ConvertSaveDataToUsableData();

            return retrivedDay;
        }
    }
}
