using CFAIProcessor.Constants;
using CFAIProcessor.Models;
using CFUtilities.Utilities;

namespace CFAIProcessor.UI.Utilities
{
    public static class UIUtilities
    {
        /// <summary>
        /// Id for Any. Used for filters on properties where no filter is currently set.
        /// </summary>
        public static string AnyId = Guid.Empty.ToString();

        private const string _anyText = "Any";

        public static void AddAny(List<AuditEventType> auditEventTypes)
        {
            auditEventTypes.Insert(0, new AuditEventType()
            {
                Id = AnyId,
                Name = _anyText
            });
        }

        public static void AddAny(List<User> users)
        {
            users.Insert(0, new User()
            {
                Id = AnyId,
                Name = _anyText
            });
        }

        /// <summary>
        /// Default date range filters
        /// </summary>
        /// <returns></returns>
        public static List<DateRangeFilter> GetDateRangeFilters()
        {
            return new List<DateRangeFilter>()
            {
                new DateRangeFilter()
                {
                    Id = "1",
                    Name = "All time"
                },
                new DateRangeFilter()
                {
                    Id = "2",
                    Name = "Today",
                    FromDate = DateTimeUtilities.GetTodayStart()
                },
                new DateRangeFilter()
                {
                    Id = "3",
                    Name = "Current week",
                    FromDate = DateTimeUtilities.GetWeekStart()
                },
                new DateRangeFilter()
                {
                    Id = "4",
                    Name = "Current month",
                    FromDate = DateTimeUtilities.GetMonthStart()
                },
                new DateRangeFilter()
                {
                    Id = "5",
                    Name = "Current year",
                    FromDate = DateTimeUtilities.GetYearStart()
                }
            };
        }

        public static List<NameAndValue> GetUserRoles()
        {
            return new List<NameAndValue>()
            {
                new NameAndValue()
                {
                    Name = UserRoleNames.Administrator,
                    Value = UserRoleNames.Administrator
                },
                new NameAndValue()
                {
                    Name = UserRoleNames.User,
                    Value = UserRoleNames.User
                },
            };
        }
    }
}
