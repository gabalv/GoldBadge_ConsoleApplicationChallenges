using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ChallengeThree.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeThree.UnitTests
{
    [TestClass]
    public class BadgeRepositoryTests
    {
        BadgeRepository _testRepo = new BadgeRepository();
        Badge _badgeOne = new Badge(12345, new List<string>() { "A1", "B2" });
        Badge _badgeTwo = new Badge();
        Badge _badgeThree = new Badge();
        Badge _badgeFour = new Badge();

        [TestInitialize]
        public void Arrange()
        {
            _badgeTwo.BadgeID = 22345;
            _badgeTwo.DoorNames = new List<string>() { "A1", "A2", "B1" };
            _badgeThree.BadgeID = 32345;
            _badgeThree.DoorNames = new List<string>() { "A2", "B1", "B2", "C1" };
            _badgeFour.BadgeID = 42345;
            _badgeFour.DoorNames = new List<string>() { "B1", "B2", "C2", "D1", "D2" };

            _testRepo.AddToDictionary(_badgeOne);
            _testRepo.AddToDictionary(_badgeTwo);
            _testRepo.AddToDictionary(_badgeThree);
            _testRepo.AddToDictionary(_badgeFour);
        }

        [DataTestMethod]
        [DataRow(12345, true)]
        [DataRow(32345, true)]
        [DataRow(45678, false)]
        public void CheckExistence_ShouldReturnEqual(int id, bool expected)
        {
            bool actual = _testRepo.CheckExistence(id);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetAllPairs_ShouldReturnFullDictionary()
        {
            Dictionary<int, List<string>> fullDict = _testRepo.GetAllPairs();

            Assert.AreEqual(fullDict.Count, 4);
        }

        [DataTestMethod]
        [DataRow(42345, true)]
        [DataRow(22345, true)]
        [DataRow(52345, false)]
        public void GetAllBadgeIDs_ShouldReturnCorrectBool(int id, bool expected)
        {
            List<int> allBadges = _testRepo.GetAllBadgeIDs();

            Assert.AreEqual(allBadges.Contains(id), expected);
        }

        [DataTestMethod]
        [DataRow(22345, true)]
        [DataRow(32345, true)]
        [DataRow(12345, false)]
        public void GetDoorsByID_ShouldReturnCorrectBool(int id, bool expected)
        {
            List<string> doors = _testRepo.GetDoorsByID(id);

            Assert.AreEqual(doors.Contains("A2"), expected);
            Assert.AreEqual(doors.Contains("B1"), expected);
        }

        [DataTestMethod]
        [DataRow(22345, true)]
        [DataRow(32345, false)]
        [DataRow(42345, false)]
        public void GetAllBadgesWithDoor_ShouldReturnCorrectBool(int id, bool expected)
        {
            Dictionary<int, List<string>> dict = _testRepo.GetAllPairs();
            Dictionary<int, List<string>> badges = _testRepo.GetAllBadgesWithDoor("A1", dict);

            Assert.AreEqual(badges.ContainsKey(id), expected);
        }

        [DataTestMethod]
        [DataRow("A1", true)]
        [DataRow("D1", true)]
        [DataRow("B1", false)]
        public void UpdateDoors_ShouldReturnCorrectBool(string door, bool expected)
        {
            _testRepo.UpdateDoors(22345, new List<string>() { "A1", "D1" });

            List<string> doors = _testRepo.GetDoorsByID(22345);
            Assert.AreEqual(doors.Contains(door), expected);
        }

        [TestMethod]
        public void AddDoor_ShouldReturnTrue()
        {
            _testRepo.AddDoor(12345, "D2");
            List<string> doors = _testRepo.GetDoorsByID(12345);

            Assert.IsTrue(doors.Contains("D2"));
            Assert.IsTrue(doors.Count == 3);
        }

        [TestMethod]
        public void AddSeveralDoors_()
        {
            List<string> doors = new List<string>() { "A1", "D2" };
            _testRepo.AddSeveralDoors(32345, doors);
            List<string> access = _testRepo.GetDoorsByID(32345);

            Assert.IsTrue(access.Contains("A1"));
            Assert.IsTrue(access.Contains("D2"));
        }

        [TestMethod]
        public void RemoveDoor_ShouldReturnCorrectBool()
        {
            _testRepo.RemoveDoor(12345, "A1");
            List<string> doors = _testRepo.GetDoorsByID(12345);

            Assert.IsFalse(doors.Contains("A1"));
            Assert.IsTrue(doors.Count == 1);
        }

        [TestMethod]
        public void DeleteBadge_ShouldReturnTrue()
        {
            bool success = _testRepo.DeleteBadge(42345);
            Dictionary<int, List<string>> newList = _testRepo.GetAllPairs();

            Assert.IsTrue(success);
            Assert.AreEqual(newList.Count, 3);
        }

        [TestMethod]
        public void DeleteDoorsFromBadge_ShouldReturnTrue()
        {
            bool success = _testRepo.DeleteDoorsFromBadge(32345);
            List<string> doors = _testRepo.GetDoorsByID(32345);

            Assert.IsTrue(success);
            Assert.IsTrue(doors.Count == 0);
        }

        [TestMethod]
        public void DeleteDoorFromAllBadges_ShouldReturnCorrectBool()
        {
            Dictionary<int, List<string>> dict = _testRepo.GetAllPairs();
            bool success = _testRepo.DeleteDoorFromAllBadges("B1");
            Dictionary<int, List<string>> badges = _testRepo.GetAllBadgesWithDoor("B1", dict);
            List<string> doors = _testRepo.GetDoorsByID(22345);

            Assert.IsTrue(badges.Count == 0);
            Assert.IsFalse(doors.Contains("B1"));
        }
    }
}
