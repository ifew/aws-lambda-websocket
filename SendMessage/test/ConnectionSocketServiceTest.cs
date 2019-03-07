using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using SendMessage;
using Microsoft.EntityFrameworkCore;

namespace SendMessage.Tests
{
    public class ConnectionSocketServiceTest
    {

        [Fact]
        public void When_List_All_Connections()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("list_all_connections_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw="
                });
            db_context.SaveChanges();

            var response = service.ListConnection();
            
            Assert.Equal(3, response.Result.Count());
            Assert.Equal("WJ4hddSkSQ0CH1A=", response.Result[0].connection_id);
            Assert.Equal("WJ46LeCjyQ0CFfg=", response.Result[1].connection_id);
            Assert.Equal("WJ463dMmyQ0AbFw=", response.Result[2].connection_id);
        }



        [Fact]
        public void When_Get_Connection_2()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("get_connection_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw="
                });
            db_context.SaveChanges();

            var response = service.GetConnection("WJ46LeCjyQ0CFfg=");

            Assert.Equal("WJ46LeCjyQ0CFfg=", response.connection_id);
            Assert.Equal(2, response.id);
        }


        [Fact]
        public void When_Delete_Connection_2()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("delete_connection_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw="
                });
            db_context.SaveChanges();

            var response = service.DeleteConnection("WJ46LeCjyQ0CFfg=");

            var response_list = service.ListConnection();

            Assert.Equal(2, response_list.Result.Count());
            Assert.Equal("WJ4hddSkSQ0CH1A=", response_list.Result[0].connection_id);
            Assert.Equal("WJ463dMmyQ0AbFw=", response_list.Result[1].connection_id);
        }

        [Fact]
        public void When_List_Connections_In_Channel()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("list_connections_in_channel_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A=",
                    channel = "secret-room"
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw=",
                    channel = "secret-room"
                });
            db_context.SaveChanges();

            var response = service.ListConnectionInChannel("secret-room");

            Assert.Equal(2, response.Result.Count());
            Assert.Equal("WJ4hddSkSQ0CH1A=", response.Result[0].connection_id);
            Assert.Equal("WJ463dMmyQ0AbFw=", response.Result[1].connection_id);
        }


        [Fact]
        public void When_Get_Specific_Connection()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("get_specific_connection_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw="
                });
            db_context.SaveChanges();

            var response = service.SendToConnection("WJ46LeCjyQ0CFfg=");

            Assert.Equal("WJ46LeCjyQ0CFfg=", response.Result.connection_id);
            Assert.Equal(2, response.Result.id);
        }


        [Fact]
        public void When_Specific_Connection_And_Channel()
        {

            var _options = new DbContextOptionsBuilder<FunctionContext>().UseInMemoryDatabase("get_specific_connection_and_channel_service").Options;
            FunctionContext db_context = new FunctionContext(_options);

            ConnectionSocketService service = new ConnectionSocketService(db_context);

            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 1,
                    connection_id = "WJ4hddSkSQ0CH1A=",
                    channel = "secret-room"
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 2,
                    connection_id = "WJ46LeCjyQ0CFfg="
                });
            db_context.Connections.Add(
                new ConnectionSocketModel
                {
                    id = 3,
                    connection_id = "WJ463dMmyQ0AbFw=",
                    channel = "secret-room"
                });
            db_context.SaveChanges();

            var response = service.SendToConnectionChannel("WJ4hddSkSQ0CH1A=", "secret-room");

            Assert.Equal("WJ4hddSkSQ0CH1A=", response.Result.connection_id);
            Assert.Equal(1, response.Result.id);
        }
    }
}
