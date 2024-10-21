using Xunit;
using Restaurants.API.Middelwares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;
using FluentAssertions;

namespace Restaurants.API.Middelwares.Tests
{
    public class ErrorHandelingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenoExceptionThrow_ShouldCallNextDelegate()
        {
            //arrange
            var loggerMock=new Mock<ILogger<ErrorHandelingMiddleware>>();
            var middleware= new ErrorHandelingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock= new Mock<RequestDelegate>();

            //
            await middleware.InvokeAsync(context,nextDelegateMock.Object);

            //
            nextDelegateMock.Verify(next=>next.Invoke(context),Times.Once);

        }
        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrow_ShouldSetStatuseCode404()
        {
            //arrange
            var loggerMock = new Mock<ILogger<ErrorHandelingMiddleware>>();
            var middleware = new ErrorHandelingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var notFoundException = new NotFoundException(nameof(Restaurant), "1");

            //act
            await middleware.InvokeAsync(context, _ => throw notFoundException);
            //assert
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidenExceptionThrow_ShouldSetStatuseCode403()
        {
            //arrange
            var loggerMock = new Mock<ILogger<ErrorHandelingMiddleware>>();
            var middleware = new ErrorHandelingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new ForbidException()  ;

            //act
            await middleware.InvokeAsync(context, _ => throw exception);
            //assert
            context.Response.StatusCode.Should().Be(403);
        }

        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrow_ShouldSetStatuseCode500()
        {
            //arrange
            var loggerMock = new Mock<ILogger<ErrorHandelingMiddleware>>();
            var middleware = new ErrorHandelingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var exception = new Exception();

            //act
            await middleware.InvokeAsync(context, _ => throw exception);
            //assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}