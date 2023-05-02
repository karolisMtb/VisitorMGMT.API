using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VisitorMGMT.API.DataAccess.DatabaseContext;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace VisitorMGMT.API.Tests.Repositories
{
    public class VisitorRepositoryTests
    {
        protected Mock<DatabaseMGMTContext> databaseContextMock;
        protected VisitorRepository visitorRepository;
        protected Mock<DbSet<Visitor>> dbSetMock;
        protected DbContextOptions<DatabaseMGMTContext> dbContextoptions;

        public VisitorRepositoryTests()
        {
            databaseContextMock = new Mock<DatabaseMGMTContext>();
            visitorRepository = new VisitorRepository(databaseContextMock.Object);
            dbSetMock = new Mock<DbSet<Visitor>>();
        }

        private DatabaseMGMTContext GetContext()
        {
            var options = new DbContextOptionsBuilder<DatabaseMGMTContext>().UseInMemoryDatabase(databaseName: "VisitorsDB").Options;
            return new DatabaseMGMTContext(options);
        }

        [Fact]
        public void Test()
        {
            //Arrange

            //Act

            //Assert
            var context = GetContext();
            var visitor = new Visitor();
            context.Visitors.Add(visitor);
            context.SaveChanges();

            Assert.True(context.Visitors.Any());

        }
    }
}


//Arrange
//Act
//Assert