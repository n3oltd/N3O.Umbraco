angular.module("umbraco")
    .controller("statistics", function ($scope, editorState, contentResource, assetsService) {


        //donations
        const donationData = {
            Total: { Amount: 26256, Currency: "GBP", Text: "£26,256" },
            Average: { Amount: 25, Currency: "GBP", Text: "£25" },
            Count: 86754
        };
        function populateTotalDonation() {
            document.getElementById('total-amount').textContent = donationData.Total.Text;
        }
        function populateAverageDonation() {
            document.getElementById('average-donation').textContent = donationData.Average.Text;
        }
        function populateDonationCount() {
            document.getElementById('donation-count').textContent = donationData.Count;
        }

        populateTotalDonation();
        populateAverageDonation();
        populateDonationCount();





        //supporters
        const supportersData = {
            TotalSupporters: "64,512",
            Oneoff: "59,845",
            Regular: "4,667"
        };

        // Function to populate total supporters
        function populateTotalSupporters() {
            document.getElementById('supporters-total-amount').textContent = supportersData.TotalSupporters;
        }

        // Function to populate one-off supporters
        function populateOneOffSupporters() {
            document.getElementById('One-off-supporters').textContent = supportersData.Oneoff;
        }

        // Function to populate regular supporters
        function populateRegularSupporters() {
            document.getElementById('Regular-supporters').textContent = supportersData.Regular;
        }

        // Call the functions to populate the data in HTML
        populateTotalSupporters();
        populateOneOffSupporters();
        populateRegularSupporters();



        
        
        
        

        //fundraiser stats
        const activepagesdata = {
            ActiveCount: "725"
        };

        function populateactivepages(data) {
            const element = document.getElementById("active-pages-amount");
            if (element) {
                element.textContent = data.ActiveCount;
            } else {
                console.error("Element with ID 'active-pages-amount' not found.");
            }
        }

        populateactivepages(activepagesdata);


        const completedpagesdata = {
            CompletedCount: "77%" 
        }

        function populatecompletedpages(data) {
            document.getElementById("completed-pages-amount").textContent = data.CompletedCount;
        }

        populatecompletedpages(completedpagesdata);



        const newpages = {
            NewCount: 256 //this isn't directly present in the model
        }

        function populatenewpages(data) {
            document.getElementById("num-new-pages").textContent = data.NewCount;
        }

        populatenewpages(newpages);



        
        
        


        //top campaigns stats
        const activeCampaignsData = {
            Count: "250"
        };
        function populateActiveCampaigns(data) {
            const element = document.getElementById("num-active-campaigns");
            if (element) {
                element.textContent = data.Count;
            } else {
                console.error("Element with ID 'active-campaigns-amount' not found.");
            }
        }
        populateActiveCampaigns(activeCampaignsData);


        const donationsPercentageData = {
            AveragePercentageComplete: "75%"
        };
        function populateDonationsPercentage(data) {
            const element = document.getElementById("total-donations-percentage");
            if (element) {
                element.textContent = data.AveragePercentageComplete;
            } else {
                console.error("Element with ID 'total-donations-percentage' not found.");
            }
        }
        populateDonationsPercentage(donationsPercentageData);




        //top fundraiser stats
        const fundraiserAmountData = {
            Count: "300"
        };

        function populateFundraiserAmount(data) {
            const element = document.getElementById("total-fundraiser-amount");
            if (element) {
                element.textContent = data.Count;
            } else {
                console.error("Element with ID 'total-fundraiser-amount' not found.");
            }
        }

        populateFundraiserAmount(fundraiserAmountData);


        const fundraiserPercentageData = {
            AveragePercentageComplete: "80%"
        };

        function populateFundraiserPercentage(data) {
            const element = document.getElementById("total-fundraiser-percentage");
            if (element) {
                element.textContent = data.AveragePercentageComplete;
            } else {
                console.error("Element with ID 'total-fundraiser-percentage' not found.");
            }
        }

        populateFundraiserPercentage(fundraiserPercentageData);






        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });