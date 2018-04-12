/*using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RankMyFilmCore.Data;
using RankMyFilmCore.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;


namespace XUnitTestRankMyFilms
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
        }
        private readonly ApplicationDbContext _context;

        [Fact]
        public async Task Get()
        {
            // Arrange
            var controller = new testLogin(_context);
            string toto = "";
            // Act
            var result =  controller.Get(id: toto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            //var person = okResult.Value.Should().BeAssignableTo<DbContext.ApplicationUser>().Subject;
            //person.Count().Should().Be(50);
        }
    
    }
    
}
*/