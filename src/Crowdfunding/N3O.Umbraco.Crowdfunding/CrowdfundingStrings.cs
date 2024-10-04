using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Crowdfunding;

public class CrowdfundingStrings : CodeStrings {

    public string Cancel => "Cancel";
    public string Save => "Save";
    public string Remove => "Remove";
    public string SearchHerePlaceholder => "Type to search...";
    public string SelectDate => "Select Date";


    public string EditGoalTitle => "Edit Goal";
    public string EditGoalDescription => "You can modify and/or add projects to your page here.";
    public string AddProject => "Add Another Project";
    public string TotalGoalAmountError => "Goals total amount should be atleast";
    public string MinimunAmountNote => "Note: Minimum amount is";
    public string AmountMultipleOf => "must have amount multiple of";
    public string RaiseGoalAmount => "I want to raise";
    public string GoalAmountRequired => "Amount is required";
    public string GoalSelectDimension1 => "Select Dimension 1";
    public string GoalSelectDimension2 => "Select Dimension 2";
    public string GoalSelectDimension3 => "Select Dimension 3";
    public string GoalSelectDimension4 => "Select Dimension 4";
    public string GoalSelectDimension => "Select Dimension";
    public string GoalProjectRequired => "Please select the project";
    public string GoalCustomFieldRequired => "%name is Required";
    

    public string ApiLoadingError => "Unable to load. Please try again";
    public string ApiLoadingSuccess => "Loaded Successfully";
    public string ApiLoading => "Loading...";
    public string ApiUpdatingError => "Unable to update. Please try again";
    public string ApiUpdatingSuccess => "Updated Successfully";
    public string ApiUpdating => "Updating...";
    
    
    public string CropperImageCropRequired => "Please edit each image to apply a required crop.";
    public string CropperImageRequired => "'Please first upload image(s)'.";
    public string CropperImageLoadError => "Unable to load the image. Please try again";
    public string CropperGalleryMinimunRequired => "Atleast add %val item(s)";
    public string CropperGalleryMaximumRequired => "Only Max %val items are allowed";
    
    public string RichTextEditorNote => "Write up to %val characters";
    public string TextAreaEditorNote => "Slightly longer text that will appear after the campaign name. You can write up to %val characters.";
    public string TextAreaEditorPlaceholder => "Type your message here";
    public string TextAreaEditorTtile => "Short Description (Optional)";
    public string TextEditorPlaceholder => "E.g. Building a school";

    

    //Creaet Fundraiser Page
    public string SuggestSlugError => "Failed to fetch suggested Slug";
    public string CreateFundraiserError => "Failed to create fundraiser";
    public string CampaignGoalOptionsError => "Failed to fetch campaign goal options";
    
    public string GenericError => "Sorry, an error has occurred. Please try again or contact support!";
    public string TryAgainError => "Something went wrong. Please try again";    
}