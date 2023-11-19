using NPoco;
using System;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace N3O.Umbraco.CrowdFunding.Konstrukt;

[TableName(CrowdfundingConstants.Tables.CrowdfundingDonations.Name)]
[PrimaryKey("Id")]
public class CrowdfundingDonations {
    [PrimaryKeyColumn(Name = CrowdfundingConstants.Tables.CrowdfundingDonations.PrimaryKey)]
    public int Id { get; set; }
    
    [Column(nameof(Reference))]
    public string Reference { get; set; }
    
    [Column(nameof(PageId))]
    public string PageId { get; set; }

    [Column(nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; }

    [Column(nameof(Amount))]
    public decimal Amount { get; set; }
    
    [Column(nameof(Currency))]
    public string Currency { get; set; }
    
    [Column(nameof(Comment))]
    public string Comment { get; set; }
    
    [Column(nameof(FirstName))]
    public string FirstName { get; set; }
    
    [Column(nameof(LastName))]
    public string LastName { get; set; }
    
    [Column(nameof(Email))]
    public string Email { get; set; }
    
    [Column(nameof(IsAnonymous))]
    public bool IsAnonymous { get; set; }
}
