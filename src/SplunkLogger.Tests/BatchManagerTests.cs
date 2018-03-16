using Microsoft.VisualStudio.TestTools.UnitTesting;
using Splunk;
using Moq;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SplunkLogger.Tests
{
    [TestClass]
    public class BatchManagerTests
    {
        //UnitOfWork_ExpectedCondition_ExpectedResult
        [TestMethod]
        public void BatchManager_WithBatchSizeOne_RaisesEventsForEachAction()
        {
            //Arrange
            var formatedMessage = new Mock<SplunkJSONEntry>();
            var mockEventAction = new Mock<Action<List<object>>>();
            var batchManager = new BatchManager(1,100, mockEventAction.Object);
            //Act
            for (int i = 0; i < 7; i++)
            {
                batchManager.Add(formatedMessage);
            }
            //Assert
            System.Threading.Thread.Sleep(1000); // manual delay to wait for async event emitter task
            mockEventAction.Verify(x => x(It.IsAny<List<object>>()), Times.Exactly(7));
            mockEventAction.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void BatchManager_WithBatchSizeTen_RaisesOneEventForTenActions()
        {
            //Arrange
            var formatedMessage = new Mock<SplunkJSONEntry>();
            var mockEventAction = new Mock<Action<List<object>>>();
            var batchManager = new BatchManager(10, 100, mockEventAction.Object);
            //Act
            for (int i = 0; i < 10; i++)
            {
                batchManager.Add(formatedMessage);
            }
            //Assert
            System.Threading.Thread.Sleep(1000); // manual delay to wait for async event emitter task
            mockEventAction.Verify(x => x(It.IsAny<List<object>>()), Times.Exactly(1));
            mockEventAction.VerifyNoOtherCalls();
        }
    }
}
