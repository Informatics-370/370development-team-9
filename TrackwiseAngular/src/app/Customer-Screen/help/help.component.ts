import { Component } from '@angular/core';

@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss']
})
export class HelpComponent {

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

  
  
}
