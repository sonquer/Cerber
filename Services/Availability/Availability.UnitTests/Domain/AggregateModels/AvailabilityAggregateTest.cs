using System;
using Availability.Domain.AggregateModels.AvailabilityRecordAggregate;
using Xunit;

namespace Availability.UnitTests.Domain.AggregateModels
{
    public class AvailabilityAggregateTest
    {
        [Fact]
        public void Availability_Constructor_AvailabilityRecordCreated()
        {
            var accountId = Guid.NewGuid();
            
            var availabilityRecord = new AvailabilityRecord(accountId, 
                "test name",
                "http://google.com", 
                200, 
                "{}",
                1);
            
            Assert.Equal(accountId, availabilityRecord.AccountId);
            Assert.Equal("test name", availabilityRecord.Name);
            Assert.Equal("http://google.com", availabilityRecord.Url);
            Assert.Equal(200, availabilityRecord.ExpectedStatusCode);
            Assert.Equal("{}", availabilityRecord.ExpectedResponse);
            Assert.Equal(1, availabilityRecord.LogLifetimeThresholdInHours);
            Assert.Equal("ST_OK", availabilityRecord.Status);
        }
        
        [Fact]
        public void Availability_HasExpectedResponse_HasExpectedResponseIsTrue()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                "{}",
                1);
            
            Assert.True(availabilityRecord.HasExpectedResponse);
        }
        
        [Fact]
        public void Availability_HasExpectedResponse_HasExpectedResponseIsFalse()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                null,
                1);
            
            Assert.False(availabilityRecord.HasExpectedResponse);
        }
        
        [Fact]
        public void Availability_AppendLog_LogCreated()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                "{}",
                1);

            availabilityRecord.AppendLog(200, "{}", 100);
            
            Assert.Single(availabilityRecord.AvailabilityLogs);
            Assert.Equal("ST_OK", availabilityRecord.Status);
        }
        
        [Fact]
        public void Availability_AppendLog_LogCreatedInvalidStatusFail()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                "{'a':'b'}",
                1);

            availabilityRecord.AppendLog(200, "{}", 100);
            
            Assert.Single(availabilityRecord.AvailabilityLogs);
            Assert.Equal("ST_ERROR", availabilityRecord.Status);
        }
        
        [Fact]
        public void Availability_AppendLog_LogCreatedInvalidResponseCodeFail()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                204, 
                "{}",
                1);

            availabilityRecord.AppendLog(200, "{}", 100);
            
            Assert.Single(availabilityRecord.AvailabilityLogs);
            Assert.Equal("ST_ERROR", availabilityRecord.Status);
        }
        
        [Fact]
        public void Availability_ClearOutdatedLogs_LogsCleared()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                null,
                0);

            availabilityRecord.AppendLog(200, "{}", 100);
            availabilityRecord.ClearOutdatedLogs();
            
            Assert.Empty(availabilityRecord.AvailabilityLogs);
        }
        
        [Fact]
        public void Availability_ClearOutdatedLogs_LogsNotRemoved()
        {
            var availabilityRecord = new AvailabilityRecord(Guid.NewGuid(), 
                "test name",
                "http://google.com", 
                200, 
                null,
                1);

            availabilityRecord.AppendLog(200, "{}", 100);
            availabilityRecord.ClearOutdatedLogs();
            
            Assert.Single(availabilityRecord.AvailabilityLogs);
        }
    }
}