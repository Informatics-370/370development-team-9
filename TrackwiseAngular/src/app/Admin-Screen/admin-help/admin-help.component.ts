import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-help',
  templateUrl: './admin-help.component.html',
  styleUrls: ['./admin-help.component.scss']
})
export class AdminHelpComponent {

  searchText: string = '';
  helpContent = [
    {
      topic: "Customer Home Page",
      content:
        "Welcome to LTT Help Centre!\n\n" +
        "- To explore our variety of trailer and truck components, click on the “Products” tab in the top navigation bar.\n" +
        "- To learn more about Limpopo Trailer & Truck, click on the “About Us” tab on the top navigation bar.\n" +
        "- To Register/Log In, please click on the “My Account” dropdown and then click on the “Register” button, if you already have an account click on the “Log In” button.\n\n" +
        "-- Please Note: You must be registered and logged in to do the following: --\n" +
        "- To view your account, click on the “My Account” dropdown and then click on the “View Profile” button.\n" +
        "- To log Out, click on the “My Account” dropdown and then click on the “Log Out” button.\n" +
        "- To view the items that are in your cart, click on the cart icon on the top navigation bar."
    },
    {
      topic: "Customer Products Page",
      content:
        "Welcome to LTT Help Centre!\n\n" +
        "- To view a product’s details, please click anywhere on the card of the product to display its details.\n" +
        "- If you want to search for a specific product, please insert your search query in the search bar below the top navigation bar.\n" +
        "- To sort the products, please select the sorting criteria by selecting one of the options provided on the left sidebar. To apply the selected sorting criteria, please click on the “Apply” button on the bottom of the sidebar.\n" +
        "- To filter the products, please select and category and/or type to filter it by on the left sidebar. To apply the filtering criteria, please click the “Apply” button on the bottom of the sidebar.\n" +
        "- To remove applied sorting and filters, click on the “Reset All” button on the bottom of the sidebar.\n" +
        "\n" +
        "-- Please Note: You must be registered and logged in to do the following: --\n" +
        "- To add a product to your cart, click on the “Add to Cart” button beneath the product. Please note that you can only add a product if there is enough stock of the product. The quantity is displayed beneath the product’s price."

    },
    {
      topic: "Customer Orders Page",
      content:
        "- To view the products of your order, click on the “View” button beside the respective order.\n" +
        "- To cancel an order that has been placed, click on the “Cancel” button beside the respective order. Please note that this only applies to current orders.\n" +
        "- To search for an order, please provide your search query in the search bar at the top of the page."
    },
    {
      topic: "Cart Page",
      content:
        "- To proceed to the checkout, click on the “Checkout” button at the bottom of the cart summary section.\n" +
        "- To remove an item from your cart, click on the “Remove” button of the product you wish to remove.\n" +
        "- To change the quantity of a product, click on the “-” or ”+” buttons beside the quantity number that is indicated.\n" +
        "\n" +
        "If the quantity does not want to increase:\n" +
        "- Please make sure that there is enough stock available of the product. This can be viewed on the Products screen. To navigate to the product screen, click on the “Products” tab on the top navigation bar.\n" +
        "- If the issue persists, log out of your account and then log back into your account. Please note that all items added to your cart will be lost when doing this!"
    },
    {
      topic: "Customer Profile",
      content:
        "- To edit your profile, click on the “Edit Profile” button on the bottom of the screen.\n" +
        "- A form will display where you can update your profile details.\n" +
        "- To save the changes you made, click on the “Save Changes” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save Changes” button before closing the page.\n" +
        "- Make sure that the Name and Surname fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "View Admin",
      content:
        "- To search for an admin, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add an admin, click on the “Add Admin” button that will navigate you to the add admin screen.\n" +
        "- To edit an admin, click on the “Edit” button beside the admin you want to edit. The system will navigate you to the edit admin screen.\n" +
        "- To remove an admin, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove an admin, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."


    },
    {
      topic: "Edit Admin",
      content:
        "- To edit admin details:\n" +
        "  - To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Admin",
      content:
        "- Enter the new admin’s details in the associated form fields.\n" +
        "- To save the admin, click the “Add Admin” button.\n" +
        "\n" +
        "If the new admin can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Admin” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Remove Admin",
      content:
        "- To remove the admin, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the admin can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "View Clients",
      content:
        "- To search for a client, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a client, click on the “Add Client” button that will navigate you to the add client screen.\n" +
        "- To edit a client, click on the “Edit” button beside the client you want to edit. The system will navigate you to the edit client screen.\n" +
        "- To remove a client, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a client, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."

    },
    {
      topic: "Edit Client",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Client",
      content:
        "- Enter the new client’s details in the associated form fields.\n" +
        "- To save the client, click the “Add Client” button.\n" +
        "\n" +
        "If the new client can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Client” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Remove Client",
      content:
        "- To remove the client, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the client can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "View Customer",
      content:
        "- To search for a client, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "\n" +
        "If you cannot execute a search query, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."

    },
    {
      topic: "View Drivers",
      content:
        "- To search for a driver, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a driver, click on the “Add Driver” button that will navigate you to the add driver screen.\n" +
        "- To edit a driver, click on the “Edit” button beside the driver you want to edit. The system will navigate you to the edit driver screen.\n" +
        "- To remove a driver, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a driver, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."


    },
    {
      topic: "Edit Driver",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Driver",
      content:
        "- Enter the new driver’s details in the associated form fields.\n" +
        "- To save the driver, click the “Add Driver” button.\n" +
        "\n" +
        "If the new driver can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Driver” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Remove Driver",
      content:
        "- To remove the driver, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the driver can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "View Products",
      content:
        "- To search for a product, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a product, click on the “Add Product” button that will navigate you to the add product screen.\n" +
        "- To edit a product, click on the “Edit” button beside the product you want to edit. The system will navigate you to the edit product screen.\n" +
        "- To remove a product, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a product, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."



    },
    {
      topic: "Edit Products",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Add Products",
      content:
        "- Enter the new Product’s details in the associated form fields.\n" +
        "- To save the Product, click the “Add Product” button.\n" +
        "\n" +
        "If the new Product can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Product” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Remove Products",
      content:
        "- To remove the product, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the product can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "View Suppliers",
      content:
        "- To search for a supplier, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a supplier, click on the “Add Supplier” button that will navigate you to the add supplier screen.\n" +
        "- To edit a supplier, click on the “Edit” button beside the supplier you want to edit. The system will navigate you to the edit supplier screen.\n" +
        "- To remove a supplier, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a supplier, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."

    },
    {
      topic: "Edit Suppliers",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Suppliers",
      content:
        "- Enter the new Supplier’s details in the associated form fields.\n" +
        "- To save the Supplier, click the “Add Supplier” button.\n" +
        "\n" +
        "If the new Supplier can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Supplier” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Remove Suppliers",
      content:
        "- To remove the supplier, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the supplier can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "View Trailers",
      content:
        "- To search for a trailer, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a trailer, click on the “Add Trailer” button that will navigate you to the add trailer screen.\n" +
        "- To edit a trailer, click on the “Edit” button beside the trailer you want to edit. The system will navigate you to the edit trailer screen.\n" +
        "- To remove a trailer, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a trailer, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."

    },
    {
      topic: "Edit Trailers",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Trailers",
      content:
        "- Enter the new trailer’s details in the associated form fields.\n" +
        "- To save the trailer, click the “Add Trailer” button.\n" +
        "\n" +
        "If the new trailer can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Trailer” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."
    },
    {
      topic: "Remove Trailers",
      content:
        "- To remove the trailer, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the trailer can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "View Trucks",
      content:
        "- To search for a truck, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To add a truck, click on the “Add Truck” button that will navigate you to the add truck screen.\n" +
        "- To edit a truck, click on the “Edit” button beside the truck you want to edit. The system will navigate you to the edit truck screen.\n" +
        "- To remove a truck, click on the remove button then confirm the removal.\n" +
        "\n" +
        "If you cannot add, edit, or remove a truck, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."


    },
    {
      topic: "Edit Trucks",
      content:
        "- To save the changes you made, click on the “Save” button.\n" +
        "\n" +
        "If your changes did not save, please do the following:\n" +
        "- Ensure that you clicked the “Save” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "Add Trucks",
      content:
        "- Enter the new truck’s details in the associated form fields.\n" +
        "- To save the truck, click the “Add Truck” button.\n" +
        "\n" +
        "If the new truck can’t be created, please do the following:\n" +
        "- Ensure that you clicked the “Add Truck” button before closing the page.\n" +
        "- Make sure that the fields are not left empty.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Remove Trucks",
      content:
        "- To remove the truck, click on the “Remove” button.\n" +
        "- Once the confirmation modal displays, click on the “Yes” button.\n" +
        "\n" +
        "If the truck can’t be removed, please do the following:\n" +
        "- Ensure that you clicked the “Yes” button on the confirmation modal.\n" +
        "- Make sure that you are connected to the internet.\n" +
        "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "View Jobs",
      content:
        "- To search for a job, enter your search query in the search bar then proceed to click on the “Search” button.\n" +
        "- To view a job, click on the “View” button besides the job.\n" +
        "\n" +
        "Job Status:\n" +
        "- If the job status says, “Job Details Required”, it means that there are documents missing related to the job.\n" +
        "\n" +
        "If you cannot view or add a job, please follow these steps:\n" +
        "- Refresh the webpage.\n" +
        "- Log out of the system and then log back into the system."


    },
    {
      topic: "View Job Details",
      content:
      "- To capture documents linked to the job, click the “Capture” button.\n" +
      "- To Mark a job as complete, click the “Complete Job” button.\n" +
      "\n" +
      "Please note:\n" +
      "- The initial mileage entered can’t be more than the final mileage.\n" +
      "\n" +
      "If you cannot capture or complete a job:\n" +
      "- Refresh the webpage.\n" +
      "- Log out of the system and then log back into the system." 

    },
    {
      topic: "View Job Details",
      content:
  "- To add a job:\n" +
  "  - Enter the new job’s details in the associated form fields.\n" +
  "  - To save the job, click the “Create Job” button.\n" +
  "\n" +
  "If the new job can’t be created, please do the following:\n" +
  "- Ensure that you clicked the “Create Job” button before closing the page.\n" +
  "- Make sure that the fields are not left empty.\n" +
  "- Make sure that you are connected to the internet.\n" +
  "- Log out of your account and then proceed to log back in afterward."


    },
    {
      topic: "View Admin Orders",
      content:
      "- To view the details associated with the order, click on the “View” button.\n" +
      "- To mark an order as complete, view the order details, then proceed to click the “Complete” button.\n" +
      "\n" +
      "If the order can’t be completed, please do the following:\n" +
      "- Ensure that you clicked the “Complete” button before closing the page.\n" +
      "- Make sure that you are connected to the internet.\n" +
      "- Log out of your account and then proceed to log back in afterward."   

    }
    ,
    {
      topic: "Generate Reports",
      content:
  "- To generate a report, click on the “Generate” button. This will open a PDF of the form you selected.\n" +
  "\n" +
  "If the new form can’t be generated, please do the following:\n" +
  "- Ensure that you clicked the “Generate” button.\n" +
  "- Make sure that you are connected to the internet.\n" +
  "- Log out of your account and then proceed to log back in afterward."

    },
    {
      topic: "Login",
      content:
  "- To log into your account, enter your email address and password in the correct fields.\n" +
  "- Then click on the “Log In” button.\n" +
  "\n" +
  "*--Please note that you need to be registered before you can log in--*\n" +
  "- If you are not yet registered, click on the “Register here” link at the bottom.\n" +
  "- If you forgot your password, click on the “Forgot password” link at the bottom.\n" +
  "\n" +
  "If you can’t log in, please try the following:\n" +
  "- Ensure that your details are correctly entered.\n" +
  "- Make sure that you are connected to the internet."


    },
    {
      topic: "Register",
      content:
  "- To register, please enter your details in the fields.\n" +
  "- To finish registering, click on the “Sign Up” button.\n" +
  "\n" +
  "If you can’t register, please try the following:\n" +
  "- Ensure that your details are correctly entered.\n" +
  "- Ensure that no fields are left empty.\n" +
  "- Make sure that the passwords match.\n" +
  "- Make sure that you are connected to the internet."

    }
  ];


  searchResults: { topic: string, content: string }[] = [];


  constructor() { }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  search() {
    if (this.searchText) {
      // Perform a search within the helpContent text
      const searchTextLower = this.searchText.toLowerCase();

      // Filter sections that contain the search text and map them to objects
      this.searchResults = this.helpContent
        .filter(section => section.content.toLowerCase().includes(searchTextLower))
        .map(section => ({ topic: section.topic, content: section.content }));
    } else {
      // If search text is empty, display all content
      this.searchResults = this.helpContent.map(section => ({ topic: section.topic, content: section.content }));
    }
  }

  downloadPdf() {
    // Replace 'your-pdf-filename.pdf' with the actual filename of your PDF in the 'assets' folder
    const pdfFilename = 'LTT-Help-Document.pdf';

    // Construct the URL to the PDF file in the 'assets' folder
    const pdfUrl = `/assets/${pdfFilename}`;

    fetch(pdfUrl)
      .then(response => response.blob())
      .then(blob => {
        const url = window.URL.createObjectURL(blob);

        // Create a hidden anchor element
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = pdfFilename; // Specify the desired file name
        document.body.appendChild(a);

        // Trigger a click event to download the file
        a.click();

        // Remove the anchor element
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
      })
      .catch(error => {
        console.error('Error downloading the PDF:', error);
      });
  }

}
