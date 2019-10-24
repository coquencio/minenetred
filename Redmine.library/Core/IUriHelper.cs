using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.Library.Core
{
    public interface IUriHelper
    {
        string Activities(int projectId, string authKey);
        string Issues(string authKey, int assignedToId, int projectId);
        string Projects(string authKey);
        string HandleTimeEntriesUri(string authKey, int userId, int projectId, string fromDate, string toDate);
        string User(string authKey);
    }
}
