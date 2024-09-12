using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class GuestFundraiserViewModel {
    public string Title { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public string ButtonText { get; set; }
    public CroppedImage Logo { get; set; }
    public IEnumerable<GuestFundraiserItemElement> Items { get; set; }

    public static GuestFundraiserViewModel For(GuestFundraiserContent guestFundraiserContent) {
        var viewModel = new GuestFundraiserViewModel();

        viewModel.Title = guestFundraiserContent?.Title;
        viewModel.Text = guestFundraiserContent?.Text;
        viewModel.Description = guestFundraiserContent?.Description;
        viewModel.Logo = guestFundraiserContent?.Logo;
        viewModel.ButtonText = guestFundraiserContent?.ButtonText;
        viewModel.Items = guestFundraiserContent?.Items;

        return viewModel;
    }
}