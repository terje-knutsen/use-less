using Akavache;
using Useless.Api;
using Useless.ApplicationService.InMemory;
using Useless.Forms.Navigation;
using Useless.Mobile.Api;
using Useless.Mobile.Pages;
using Useless.Mobile.ViewModels;
using UseLess.Messages;

namespace Useless.Mobile.Extensions
{
    public static class MauiAppBuilderExtensions
    {
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder) 
        {
            builder.Services.AddTransient<StartupViewModel>();
            builder.Services.AddTransient<IncomesViewModel>();
            builder.Services.AddTransient<AddIncomeViewModel>();
            builder.Services.AddTransient<OutgosViewModel>();
            builder.Services.AddTransient<EditIncomeViewModel>();
            builder.Services.AddTransient<EditOutgoViewModel>();
            builder.Services.AddTransient<EditExpenseViewModel>();
            builder.Services.AddTransient<ExpensesViewModel>();
            builder.Services.AddTransient<AddExpenseViewModel>();
            builder.Services.AddTransient<EditBudgetViewModel>();
            return builder;
        }
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton(x => BlobCache.LocalMachine);
            builder.Services.AddTransient<IApplyBudgetCommand, LocalCommandApplier>();
            builder.Services.AddTransient<IProjection<ReadModels.Budget, QueryModels.GetBudget>, LocalProjection>();
            builder.Services.AddTransient<IProjection<ReadModels.Income,QueryModels.GetIncome>, LocalProjection>();
            builder.Services.AddTransient<IProjection<ReadModels.Expense, QueryModels.GetExpense>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.Income, QueryModels.GetIncomes>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.Budget, QueryModels.GetBudgets>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.Outgo, QueryModels.GetOutgos>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.Expense, QueryModels.GetExpenses>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.IncomeType, QueryModels.GetIncomeTypes>, LocalProjection>();
            builder.Services.AddTransient<ICollectionProjection<ReadModels.OutgoType, QueryModels.GetOutgoTypes>, LocalProjection>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            return builder;
        }
        public static MauiAppBuilder RegisterRoutes(this MauiAppBuilder builder)
        {
            MauiProgram.AddPage<StartupPage, StartupViewModel>(builder.Services);
            MauiProgram.AddPage<IncomeCollectionPage, IncomesViewModel>(builder.Services);
            MauiProgram.AddPage<EditIncomePage, EditIncomeViewModel>   (builder.Services);
            MauiProgram.AddPage<IncomeRegistrationPage, AddIncomeViewModel> (builder.Services);
            MauiProgram.AddPage<OutgoCollectionPage, OutgosViewModel>(builder.Services);
            MauiProgram.AddPage<OutgoRegistrationPage, AddOutgoViewModel>(builder.Services);
            MauiProgram.AddPage<EditPage, EditOutgoViewModel> (builder.Services);
            MauiProgram.AddPage<CollectionPage, ExpensesViewModel>(builder.Services);
            MauiProgram.AddPage<ExpenseRegistrationPage,AddExpenseViewModel>(builder.Services);
            MauiProgram.AddPage<EditPage, EditExpenseViewModel>(builder.Services);
            MauiProgram.AddPage<EditBudgetPage, EditBudgetViewModel> (builder.Services);
            return builder;
        }
    }
}
