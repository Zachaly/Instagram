using Instagram.Domain.SqlAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Models.UserBan.Request
{
    public class GetUserBanRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? StartDate { get; set; }
        public long? EndDate { get; set; }
        public long? UserId { get; set; }

        [Where(Condition = "[UserBan].[EndDate]<=")]
        public long? MinEndDate { get; set; }
    }
}
