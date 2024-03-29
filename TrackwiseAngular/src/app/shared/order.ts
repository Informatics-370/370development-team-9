import { Customer } from "./customer";

export interface Order{

    order_ID: string;
    date: Date;
    total: number;
    status: string;
    customer_ID: string;
    customer?: Customer[];
    orderLines: OrderLine[];
}

export interface OrderLine {
    order_line_ID: string;
    
    product: {
      product_ID: string;
      product_Name: string;
      product_Description: string;
      product_Price: number;
      quantity: number;
      cartQuantity?: number;

      productType: {
        product_Type_ID: string;
        name: string;
        description: string;
      };
      productCategory: {
        product_Category_ID: string;
        name: string;
        description: string;
      };
    };
    quantity: number;
    subTotal: number;
  }
