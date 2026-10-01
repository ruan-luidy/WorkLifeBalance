using System;
namespace WorkLifeBalance.Shared.Data
{
    [Serializable]
    public class ProcessActivityData : ActivityDataBase
    {
        public override string Date { get; set; } = "06062023";
        public string Process { get; set; } = "";
        public override string TimeSpent { get; set; } = "000000";
    }
}
