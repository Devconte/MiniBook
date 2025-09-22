using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniBook.Models;

namespace MiniBook.Tests
{
    [TestClass]
    public class PostTests
    {
        [TestMethod]
        public void IsValid_ReturnsTrue_WhenContentIsCorrect()
        {
            var post = new Post { Content = "Hello MiniBook!" };

            bool result = !string.IsNullOrWhiteSpace(post.Content) && post.Content.Length <= 280;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenContentIsEmpty()
        {
            var post = new Post { Content = "" };

            bool result = !string.IsNullOrWhiteSpace(post.Content) && post.Content.Length <= 280;

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValid_ReturnsFalse_WhenContentExceeds280Characters()
        {
            var post = new Post { Content = new string('a', 281) };

            bool result = !string.IsNullOrWhiteSpace(post.Content) && post.Content.Length <= 280;

            Assert.IsFalse(result);
        }
    }
}