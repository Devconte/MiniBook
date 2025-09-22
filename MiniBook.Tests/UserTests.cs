using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniBook.Models;

namespace MiniBook.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void AddFriend_ShouldAddFriendship_ToCollection()
        {
            // Arrange
            var alice = new User { Id = 1, UserName = "Alice", Email = "alice@test.com" };
            var bob = new User { Id = 2, UserName = "Bob", Email = "bob@test.com" };

            // Act
            var friendship = alice.AddFriend(bob);

            // Assert
            Assert.AreEqual(1, alice.FriendshipsSent.Count);
            Assert.AreEqual(alice, friendship.Requester);
            Assert.AreEqual(bob, friendship.Addressee);
            Assert.AreEqual(FriendshipStatus.Pending, friendship.Status);
        }

        [TestMethod]
        public void AddFriend_ShouldThrowInvalidOperationException_WhenAddingSelf()
        {
            var alice = new User { Id = 1, UserName = "Alice", Email = "alice@test.com" };

            Assert.ThrowsException<InvalidOperationException>(() => alice.AddFriend(alice));
        }

        [TestMethod]
        public void AddFriend_ShouldThrowArgumentNullException_WhenAddresseeIsNull()
        {
            var alice = new User { Id = 1, UserName = "Alice", Email = "alice@test.com" };

            Assert.ThrowsException<ArgumentNullException>(() => alice.AddFriend(null!));
        }
    }
}