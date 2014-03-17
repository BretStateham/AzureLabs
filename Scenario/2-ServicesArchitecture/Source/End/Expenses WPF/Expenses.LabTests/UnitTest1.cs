using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Expenses.Model;
using Expenses.Model.Fakes;
using Expenses.ViewModel;
using Expenses.ViewModel.Fakes;
using System.Threading.Tasks;


namespace Expenses.LabTests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public async Task TestMethod1()
    {
      ServiceLocator.Current.SetService<INavigationService>(
        new StubINavigationService());

      StubIViewService viewService =
        new StubIViewService()
        {
          ExecuteBusyActionAsyncFuncOfTask =
              async (Func<Task> func) =>
              {
                await func();
              }
        };
      ServiceLocator.Current.SetService<IViewService>(viewService);

      StubIExpenseRepository repository =
        new StubIExpenseRepository()
        {
          GetChargeAsyncInt32 =
              (chargeId) =>
              {
                return Task.FromResult(
                    new Charge()
                    {
                      ChargeId = chargeId,
                    });
              }
        };
      ServiceLocator.Current.SetService<IExpenseRepository>(repository);




      // Create a new ChargeViewModel.
      ChargeViewModel chargeViewModel = new ChargeViewModel();

      // Make sure it defaults to a ChargeId of 0.
      Assert.AreEqual(0, chargeViewModel.ChargeId);

      // Load the charge with the ChargeId of 1.
      await chargeViewModel.LoadAsync(1);

      // Confirm the ViewModel’s ChargeId is 1.
      Assert.AreEqual(1, chargeViewModel.ChargeId);

    }
  }
}
