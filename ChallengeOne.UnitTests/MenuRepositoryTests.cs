using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ChallengeOne.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChallengeOne.UnitTests
{
    [TestClass]
    public class MenuRepositoryTests
    {
        private MenuRepository _repo;
        private MenuItem _itemOne;
        private MenuItem _itemTwo;
        private MenuItem _itemThree;
        private MenuItem _itemFour;

        [TestInitialize]
        public void Arrange()
        {
            _repo = new MenuRepository();
            _itemOne = new MenuItem(1, "Sandwich", "Meat and cheese in between two pieces of bread.", new List<string>() { "meat", "cheese", "bread" }, 19.20);
            _itemTwo = new MenuItem(2, "Cheesy Fried Eggs", "Cheese on eggs fried in butter.", new List<string>() { "eggs", "butter", "cheese" }, 11.20);
            _itemThree = new MenuItem(3, "Coffee", "You know, the classic drink.", new List<string>() { "hot water", "coffee grounds" }, 4.99);
            _itemFour = new MenuItem(4, "Fried Rice", "Stir-fried rice mixed with various ingredients.", new List<string>() { "rice", "eggs", "cooking oil", "garlic", "onions" }, 30);
            _repo.AddMenuItem(_itemOne);
            _repo.AddMenuItem(_itemTwo);
            _repo.AddMenuItem(_itemThree);
            _repo.AddMenuItem(_itemFour);
        }

        [TestMethod]
        public void GetAllItems_ShouldReturnTrue()
        {
            List<MenuItem> menu = _repo.GetAllItems();
            bool menuHasEggs = menu.Contains(_itemTwo);
            bool menuHasCoffee = menu.Contains(_itemThree);

            Assert.IsTrue(menuHasEggs);
            Assert.IsTrue(menuHasCoffee);
        }

        [DataTestMethod]
        [DataRow(3, 3)]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        public void GetItemByNumber_ShouldReturnEqual(int numOne, int expectedNum)
        {
            MenuItem item = _repo.GetItemByNumber(numOne);

            Assert.AreEqual(item.Number, expectedNum);
        }

        [DataTestMethod]
        [DataRow("Coffee", "Coffee")]
        [DataRow("Fried Rice", "Fried Rice")]
        [DataRow("Cheesy Fried Eggs", "Cheesy Fried Eggs")]
        public void GetItemByName_ShouldReturnEqual(string name, string expectedName)
        {
            MenuItem item = _repo.GetItemByName(name);

            Assert.AreEqual(item.Name, expectedName);
        }

        [TestMethod]
        public void GetAllItemsUnderPrice_ShouldReturnFalse()
        {
            List<MenuItem> itemsUnderTwenty = _repo.GetAllItemsUnderPrice(20);

            Assert.IsFalse(itemsUnderTwenty.Contains(_itemFour));
        }

        [DataTestMethod]
        [DataRow("cheese", 2, 3)]
        [DataRow("eggs", 4, 1)]
        public void GetAllItemsWithIngredient_ShouldReturnCorrectBool(string ingredient, int numOne, int numTwo)
        {
            List<MenuItem> itemsWithIngredient = _repo.GetAllItemsWithIngredient(ingredient);
            MenuItem expectedItem = _repo.GetItemByNumber(numOne);
            MenuItem nonExpectedItem = _repo.GetItemByNumber(numTwo);

            Assert.IsTrue(itemsWithIngredient.Contains(expectedItem));
            Assert.IsFalse(itemsWithIngredient.Contains(nonExpectedItem));
        }

        [TestMethod]
        public void UpdateMenuItem_ShouldReturnEqual()
        {
            MenuItem item = new MenuItem();
            item.Number = 5;

            _repo.UpdateMenuItem(item, "Coffee");
            List<MenuItem> updatedList = _repo.GetAllItems();

            Assert.AreEqual(updatedList[2].Number, 5);
        }

        [TestMethod]
        public void DeleteItemByNumber_ShouldReturnTrue()
        {
            Assert.IsTrue(_repo.DeleteItemByNumber(3));
        }

        [TestMethod]
        public void DeleteItemByName_ShouldReturnTrue()
        {
            Assert.IsTrue(_repo.DeleteItemByName("Sandwich"));
        }

        [DataTestMethod]
        [DataRow("cheese", 3, 1)]
        [DataRow("eggs", 1, 4)]
        public void DeleteItemsByIngredient_ShouldReturnCorrectBool(string ingredient, int numOne, int numTwo)
        {
            _repo.DeleteItemsByIngredient(ingredient);
            List<MenuItem> itemsWithoutIngredient = _repo.GetAllItems();
            MenuItem expected = _repo.GetItemByNumber(numOne);
            MenuItem nonExpected = _repo.GetItemByNumber(numTwo);

            Assert.IsTrue(itemsWithoutIngredient.Contains(expected));
            Assert.IsFalse(itemsWithoutIngredient.Contains(nonExpected));
        }
    }
}
