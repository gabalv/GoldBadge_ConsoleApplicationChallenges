using System;
using System.Collections.Generic;
using ChallengeTwo.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeTwo.UnitTests
{
    [TestClass]
    public class ClaimRepositoryTests
    {
        private ClaimRepository _testRepo;
        private Claim _claimOne;
        private Claim _claimTwo;
        private Claim _claimThree;
        private Claim _claimFour;

        [TestInitialize]
        public void Arrange()
        {
            _testRepo = new ClaimRepository();
            _claimOne = new Claim(1, ClaimType.Car, "Car crash", 2000, new DateTime(2010, 1, 1), new DateTime(2010, 1, 25));
            _claimTwo = new Claim(2, ClaimType.Home, "Break-in", 50000, new DateTime(2011, 6, 30), new DateTime(2011, 8, 2));
            _claimThree = new Claim(3, ClaimType.Theft, "Lost Car Keys", 50, new DateTime(2011, 2, 27), new DateTime(2011, 3, 14));
            _claimFour = new Claim(4, ClaimType.Theft, "Kidnapped Dog", 3500, new DateTime(2012, 9, 8), new DateTime(2012, 10, 21));

            _testRepo.AddClaim(_claimOne);
            _testRepo.AddClaim(_claimTwo);
            _testRepo.AddClaim(_claimThree);
            _testRepo.AddClaim(_claimFour);
        }

        [TestMethod]
        public void GetAllClaims_ShouldReturnCorrectBool()
        {
            List<Claim> allClaims = _testRepo.GetAllClaims();
            bool hasClaimTwo = allClaims.Contains(_claimTwo);
            bool hasClaimThree = allClaims.Contains(_claimThree);

            Assert.IsTrue(hasClaimTwo);
            Assert.IsTrue(hasClaimThree);
        }

        [DataTestMethod]
        [DataRow(2, 50000)]
        [DataRow(4, 3500)]
        public void GetClaimByID_ShouldReturnEqual(int id, double expectedClaimAmount)
        {
            Claim actualClaim = _testRepo.GetClaimByID(id);

            Assert.AreEqual(actualClaim.ClaimAmount, expectedClaimAmount);
        }

        [TestMethod]
        public void UpdateClaim_ShouldReturnCorrectBool()
        {
            Claim newClaim = new Claim(9, ClaimType.Car, "Car break-in", 3000, new DateTime(2000, 1, 20), new DateTime(2002, 3, 20));
            _testRepo.UpdateClaim(3, newClaim);
            List<Claim> newList = _testRepo.GetAllClaims();

            Assert.IsTrue(newList.Contains(newClaim));
            Assert.IsFalse(newList.Contains(_claimThree));
        }

        [DataTestMethod]
        [DataRow(2, true)]
        [DataRow(7, false)]
        public void DeleteClaimByID_ShouldReturnCorrectBool(int id, bool expected)
        {
            bool check = _testRepo.DeleteClaimByID(id);

            Assert.AreEqual(check, expected);
            Assert.AreEqual(_testRepo.GetClaimByID(id), null);
        }
    }
}
