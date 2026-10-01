using System;
using System.Threading.Tasks;
namespace WorkLifeBalance.Shared.Navigation;

public interface IWindowService<in T> where T: PageViewModelBase
{
    Task Close();
    Task OpenWith<Tvm>(object? args = null) where Tvm : PageViewModelBase;
    Action? OnPageLoaded { get; set; }
}