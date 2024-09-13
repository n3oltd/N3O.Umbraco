using N3O.Umbraco.Cropper.Models;
using N3O.Umbraco.Crowdfunding.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.CrowdFunding.Models;

public class GuestFundraiserViewModel {
    public string Title { get; private set; }
    public string Text { get; private set; }
    public string Description { get; private set; }
    public string ButtonText { get; private set; }
    public CroppedImage Logo { get; private set; }
    public IEnumerable<GuestFundraiserItemElement> Items { get; private set; }

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