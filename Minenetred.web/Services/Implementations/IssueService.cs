using AutoMapper;
using Minenetred.Web.Context;
using Minenetred.Web.Models;
using Redmine.Library.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minenetred.Web.Services.Implementations
{
    public class IssueService : IIssueService
    {
        private readonly MinenetredContext _context;
        private readonly Redmine.Library.Services.IIssueService _issueService;
        private readonly IMapper _mapper;
        private IUsersManagementService _usersManagementService;

        public IssueService(
            MinenetredContext context,
            Redmine.Library.Services.IIssueService issueService,
            IMapper mapper,
            IUsersManagementService usersManagementService
            )
        {
            _context = context;
            _issueService = issueService;
            _mapper = mapper;
            _usersManagementService = usersManagementService;
        }

        public async Task<List<IssueDto>> GetIssuesAsync(int projectId, string email)
        {
            var userEmail = email;
            var user = _context.Users.SingleOrDefault(u => u.UserName == userEmail);
            var decryptedKey = _usersManagementService.GetUserKey(userEmail);
            var response = await _issueService.GetIssuesAsync(decryptedKey, user.RedmineId, projectId);
            var toReturn = _mapper.Map<List<Issue>, List<IssueDto>>(response);
            return toReturn;
        }
    }
}