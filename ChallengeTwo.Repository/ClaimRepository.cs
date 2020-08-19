using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeTwo.Repository
{
    public class ClaimRepository
    {
        private readonly List<Claim> _claimrepo = new List<Claim>();

        //CREATE
        public void AddClaim(Claim claim)
        {
            _claimrepo.Add(claim);
        }

        //READ
        public List<Claim> GetAllClaims()
        {
            return _claimrepo;
        }

        public Claim GetClaimByID(int id)
        {
            foreach (Claim claim in _claimrepo)
            {
                if (claim.ClaimID == id)
                {
                    return claim;
                }
            }
            return null;
        }

        //UPDATE
        public bool UpdateClaim(int id, Claim updatedClaim)
        {
            Claim targetClaim = GetClaimByID(id);
            if (targetClaim != null)
            {
                int itemIndex = _claimrepo.IndexOf(targetClaim);
                _claimrepo[itemIndex] = updatedClaim;
                return true;
            }
            return false;
        }

        //DELETE
        public bool DeleteClaim(Claim claim)
        {
            return _claimrepo.Remove(claim);
        }

        public bool DeleteClaimByID(int id)
        {
            Claim targetClaim = GetClaimByID(id);
            return DeleteClaim(targetClaim);
        }
    }
}
