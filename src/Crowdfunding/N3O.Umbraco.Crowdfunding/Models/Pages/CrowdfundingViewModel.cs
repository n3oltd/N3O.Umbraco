namespace N3O.Umbraco.CrowdFunding.Models;

public abstract class CrowdfundingViewModel<T> {
    public T Content { get; set; }
}