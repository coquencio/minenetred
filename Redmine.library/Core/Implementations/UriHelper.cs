namespace Redmine.Library.Core
{
    public class UriHelper : IUriHelper
    {
        public string Activities(int projectId, string authKey)
        {
            return Constants.Activites +
                Constants.Json +
                "?" +
                Constants.Key +
                authKey +
                "&" +
                Constants.ProjectId +
                projectId;
        }

        public string Issues(string authKey, int assignedToId, int projectId)
        {
            return Constants.Issues +
                Constants.Json +
                "?" +
                Constants.Key +
                authKey +
                "&" +
                Constants.AssignedTo +
                assignedToId +
                "&" +
                Constants.ProjectId +
                projectId;
        }

        public string Projects(string authKey)
        {
            return Constants.Projects +
                Constants.Json +
                "?" +
                Constants.Key +
                authKey;
        }

        private string TimeEntry(string authKey, int userId)
        {
            string toReturn =
                Constants.TimeEntries +
                Constants.Json +
                "?" +
                Constants.Key +
                authKey +
                "&" +
                Constants.UserId +
                userId;
            return toReturn;
        }

        private string TimeEntry(string authKey, int userId, int projectId)
        {
            string toReturn =
                TimeEntry(authKey, userId);
            toReturn +=
                "&" +
                Constants.ProjectId +
                projectId;
            return toReturn;
        }

        private string TimeEntry(string authKey, int userId, int projectId, string fromDate)
        {
            string toReturn =
                TimeEntry(authKey, userId, projectId);
            toReturn += 
                "&from=" +
                fromDate;
            return toReturn;
        }

        private string TimeEntry(string authKey, int userId, int projectId, string fromDate, string toDate)
        {
            string toReturn =
                TimeEntry(authKey, userId, projectId, fromDate);
            toReturn +=
                "&to=" +
                toDate;
            return toReturn;
        }

        private string TimeEntry(string authKey, int userId, string fromDate, string toDate)
        {
            string toReturn = TimeEntry(authKey, userId);
            toReturn +=
                "&" +
                "from=" +
                fromDate +
                "&to=" +
                toDate;
            return toReturn;
        }

        public string HandleTimeEntriesUri(string authKey, int userId, int projectId, string fromDate, string toDate)
        {
            if (projectId == 0 && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                return TimeEntry(authKey, userId);
            }
            else if (projectId != 0 && string.IsNullOrEmpty(fromDate) && string.IsNullOrEmpty(toDate))
            {
                return TimeEntry(authKey, userId, projectId);
            }
            else if (projectId != 0 && string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                return TimeEntry(authKey, userId, projectId, fromDate);
            }
            else if (projectId == 0 && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                return TimeEntry(authKey, userId, fromDate, toDate);
            }
            else
            {
                return TimeEntry(authKey, userId, projectId, fromDate, toDate);
            }
        }

        public string User(string authKey)
        {
            return Constants.CurrentUser +
                Constants.Json +
                "?" +
                Constants.Key +
                authKey;
        }
    }
}