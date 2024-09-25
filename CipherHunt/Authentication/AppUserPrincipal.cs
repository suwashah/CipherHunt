using Repository.CommonClass;
using System.Security.Claims;

namespace CipherHunt.Authentication
{
    public class AppUserPrincipal : ClaimsPrincipal
    {
        public AppUserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }
        public string ID
        {
            get
            {
                return this.FindFirst(ClaimTypes.Sid).Value;
            }
        }
        public string Token
        {
            get
            {
                return this.FindFirst(ClaimTypes.Hash).Value;
            }
        }
        public string AgentCode
        {
            get
            {
                return this.FindFirst(Strings.ClaimType.AgentCode).Value;
            }
        }
        public string AgentBranchCode
        {
            get
            {
                return this.FindFirst(Strings.ClaimType.AgentBranchCode).Value;
            }
        }
        public string BranchCodeChar
        {
            get
            {
                return this.FindFirst(Strings.ClaimType.BranchCodeChar).Value;
            }
        }
    }

    public class CPanelAppUserPrincipal : ClaimsPrincipal
    {
        public CPanelAppUserPrincipal(ClaimsPrincipal principal) : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }
        public string ID
        {
            get
            {
                return this.FindFirst(ClaimTypes.Sid).Value;
            }
        }
        public string Token
        {
            get
            {
                return this.FindFirst(ClaimTypes.Hash).Value;
            }
        }
    }
}