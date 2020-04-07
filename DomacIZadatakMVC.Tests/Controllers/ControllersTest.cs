using DomaciZadatakMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;


namespace DomacIZadatakMVC.Tests.Controllers
{
    [TestClass]
    public class ControllersTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            ViewResult result = controller.Index() as ViewResult;

            //Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(result);
        }

        

    }
}
