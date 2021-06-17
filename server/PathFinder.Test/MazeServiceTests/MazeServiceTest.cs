﻿using System;
using Moq;
using NUnit.Framework;
using PathFinder.DataAccess1;
using PathFinder.DataAccess1.Implementations;
using PathFinder.Domain.Models.MazeCreation;
using PathFinder.Domain.Services.MazeService;

namespace PathFinder.Test.MazeServiceTests
{
    [TestFixture]
    public class MazeServiceTest
    {
        [Test]
        public void Test_CannotAddToRepositoryTwice()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.GetAvailableNames())
                .Returns(() => new [] { "Kruskal"});

            var service = new MazeService(new MazeRepository(), creationFactory.Object);
            
            service.Add("test", new int[,]{});
            Assert.Throws<ArgumentException>(() => service.Add("test", new int[,] { }));
        }

        [Test]
        public void Test_CannotAddToRepositoryWhenExistsInCreationFactory()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.GetAvailableNames())
                .Returns(() => new [] { "Kruskal"});

            var service = new MazeService(new MazeRepository(), creationFactory.Object);
            
            Assert.Throws<ArgumentException>(() => service.Add("Kruskal", new int[,] { }));
        }

        [Test]
        public void Test_GetAvailableNames()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.GetAvailableNames())
                .Returns(() => new [] { "Kruskal"});

            var repository = new Mock<IMazeRepository>();
            repository.Setup(x => x.GetMazesNames())
                .Returns(() => new[] {"test", "test2"});

            var service = new MazeService(repository.Object, creationFactory.Object);
            
            CollectionAssert.AreEquivalent(new [] {"Kruskal", "test", "test2"}, service.GetAvailableNames());
        }

        [Test]
        public void Test_GetMazeWhenNotExists()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.Create(It.IsAny<string>()))
                .Returns(() => null);

            var repository = new Mock<IMazeRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(() => null);
            
            var service = new MazeService(repository.Object, creationFactory.Object);
            Assert.Throws<ArgumentException>(() => service.Get("test"));
        }

        [Test]
        public void Test_GetMazeFromFactory()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.Create("test"))
                .Returns(() => new int[1,1]);

            var repository = new Mock<IMazeRepository>();
            repository.Setup(x => x.Get(It.IsAny<string>()))
                .Returns(() => null);
            
            var service = new MazeService(repository.Object, creationFactory.Object);
            CollectionAssert.AreEquivalent(new int[1, 1], service.Get("test"));
        }

        [Test]
        public void Test_GetMazeFromRepository()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.Create(It.IsAny<string>()))
                .Returns(() => null);

            var repository = new Mock<IMazeRepository>();
            repository.Setup(x => x.Get("test"))
                .Returns(() => new int[1, 1]);
            
            var service = new MazeService(repository.Object, creationFactory.Object);
            CollectionAssert.AreEquivalent(new int[1, 1], service.Get("test"));
        }

        [Test]
        public void Test_WhenMazeExistsInRepositoryAndInFactoryReturnsRepositoryVariant()
        {
            var creationFactory = new Mock<IMazeCreationFactory>();
            creationFactory.Setup(x => x.Create("test"))
                .Returns(() => new int[2, 2]);

            var repository = new Mock<IMazeRepository>();
            repository.Setup(x => x.Get("test"))
                .Returns(() => new int[1, 1]);
            
            var service = new MazeService(repository.Object, creationFactory.Object);
            CollectionAssert.AreEquivalent(new int[1, 1], service.Get("test"));
        }
    }
}