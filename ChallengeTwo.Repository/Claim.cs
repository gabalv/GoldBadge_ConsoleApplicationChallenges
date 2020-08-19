using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeTwo.Repository
{
    public enum ClaimType { Unspecified, Car, Home, Theft }
    public class Claim
    {
        public Claim() { }
        public Claim(int claimId, ClaimType claimType, string description, double claimAmount, DateTime dateOfIncident, DateTime dateOfClaim)
        {
            ClaimID = claimId;
            ClaimType = claimType;
            Description = description;
            ClaimAmount = claimAmount;
            DateOfIncident = dateOfIncident;
            DateOfClaim = dateOfClaim;
        }
        public int ClaimID { get; set; }
        public ClaimType ClaimType { get; set; }
        public string Description { get; set; }
        public double ClaimAmount { get; set; }
        public DateTime DateOfIncident { get; set; }
        public DateTime DateOfClaim { get; set; }
        public DateTime TimeOfCompletion { get; set; }
        public bool IsValid
        {
            get
            {
                TimeSpan valid = DateOfClaim - DateOfIncident;
                if (valid.TotalDays < 31)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
