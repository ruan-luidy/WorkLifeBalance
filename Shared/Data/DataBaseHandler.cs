using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
namespace WorkLifeBalance.Shared.Data
{
    public class DataBaseHandler
    {
        private readonly SqlDataAccess dataAccess;

        public DataBaseHandler(SqlDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task WriteAutoSateData(AutoStateChangeData autod)
        {
            autod.ConvertUsableDataToSaveData();

            //I'm too dumb to figure out the sql for less calls
            string DeleteOldWorkingWindowsSQL = @"DELETE FROM WorkingWindows";

            await dataAccess.ExecuteAsync(DeleteOldWorkingWindowsSQL, new { });

            string DeleteOldWorkingPagesSSql = @"DELETE FROM WorkingUrls";

            await dataAccess.ExecuteAsync(DeleteOldWorkingPagesSSql, new { });
            
            string InsertNewWorkingWindowsSQL = @"INSERT INTO WorkingWindows (WorkingStateWindows)
                  VALUES (@WorkingWindow)";

            foreach(string window in autod.WorkingStateWindows)
            {
                await dataAccess.WriteDataAsync(InsertNewWorkingWindowsSQL, new { WorkingWindow = window });
            }
            
            string insertNewWorkingPagesSql = @"INSERT INTO WorkingUrls (WorkingStateUrl)
                  VALUES (@WorkingUrl)";

            foreach (string url in autod.WorkingStateUrls)
            {
                await dataAccess.WriteDataAsync(insertNewWorkingPagesSql, new { WorkingUrl = url });
            }

            await InsertActivity(autod.ProcessActivities, "Process");
            await InsertActivity(autod.PageActivities, "Url");
        }

        public async Task<AutoStateChangeData> ReadAutoStateData(string date)
        {
            AutoStateChangeData retrivedSettings = new();

            string sql = @$"SELECT Date, Process, TimeSpent FROM Activity
                            WHERE Date = @Date";

            retrivedSettings.ProcessActivities = (await dataAccess.ReadDataAsync<ProcessActivityData, dynamic>(sql, new { Date = date })).ToArray();

            sql = @$"SELECT Date, Url, TimeSpent FROM Activity
                            WHERE Date = @Date";
            
            retrivedSettings.PageActivities = (await dataAccess.ReadDataAsync<PageActivityData, dynamic>(sql, new { Date = date })).ToArray();
            
            sql = @$"SELECT WorkingStateWindows FROM WorkingWindows";

            retrivedSettings.WorkingStateWindows = (await dataAccess.ReadDataAsync<string, dynamic>(sql, new { })).ToArray();

            sql = @"SELECT WorkingStateUrl FROM WorkingUrls";
            
            retrivedSettings.WorkingStateUrls = (await dataAccess.ReadDataAsync<string, dynamic>(sql, new { })).ToArray();
            
            retrivedSettings.ConvertSaveDataToUsableData();
            
            return retrivedSettings;
        }

        public async Task WriteSettings(AppSettingsData sett)
        {
            sett.ConvertUsableDataToSaveData();

            string sql = @"UPDATE Settings 
                        SET LastTimeOpened = @LastTimeOpened,
                        StartWithWindows = @StartWithWindows,
                        SaveInterval = @SaveInterval,
                        AutoDetectInterval = @AutoDetectInterval,
                        AutoDetectIdleInterval = @AutoDetectIdleInterval,
                        MinimizeToTray = @MinimizeToTray
                        LIMIT 1";

            await dataAccess.WriteDataAsync(sql, sett);
        }

        public async Task<AppSettingsData> ReadSettings()
        {
            AppSettingsData? retrivedSettings;

            string sql = @$"SELECT * FROM Settings
                            LIMIT 1";

            retrivedSettings = (await dataAccess.ReadDataAsync<AppSettingsData, dynamic>(sql, new { })).FirstOrDefault();

            retrivedSettings ??= new();

            retrivedSettings.ConvertSaveDataToUsableData();

            return retrivedSettings;
        }

        public async Task WriteDay(DayData day)
        {
            day.ConvertUsableDataToSaveData();
            
            string sql = @"INSERT OR REPLACE INTO Days (Date,WorkedAmmount,RestedAmmount,IdleAmmount)
                         VALUES (@Date,@WorkedAmmount,@RestedAmmount,@IdleAmmount)";

            await dataAccess.WriteDataAsync(sql, day);
        }

        public async Task<DayData> ReadDay(string date)
        {
            DayData? retrivedDay;

            string sql = @$"SELECT * FROM Days
                          WHERE Date = @Date";
            retrivedDay = (await dataAccess.ReadDataAsync<DayData, dynamic>(sql, new { Date = date })).FirstOrDefault();

            retrivedDay ??= new();

            retrivedDay.ConvertSaveDataToUsableData();

            return retrivedDay;
        }

        private async Task InsertActivity<T>(T[] items, string column) where T : ActivityDataBase
        {
            
            string updateActivitySql = @$"UPDATE Activity
                                SET TimeSpent = @TimeSpent
                                WHERE Date = @Date AND {column} = @{column}";

            string insertActivitySql = @$"INSERT INTO Activity (Date,{column},TimeSpent)
                               VALUES (@Date,@{column},@TimeSpent)";
            
            foreach (T activity in items)
            {
                int affectedRows = await dataAccess.WriteDataAsync(updateActivitySql, activity);

                if (affectedRows == 0)
                {
                    await dataAccess.WriteDataAsync(insertActivitySql, activity);
                }
            }
        }
    }
}