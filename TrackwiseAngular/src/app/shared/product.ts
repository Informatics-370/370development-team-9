export interface Product {
    product_ID: string;
    product_Name: string;
    product_Description: string;
    product_Price:number;
    product_Category_ID:string;
    productCategory:{
        product_Category_ID:string;
        name:string;
        description:string;
    };
    product_Type_ID:string;
        productType:{
        product_Type_ID:string;
        name:string;
        description:string;
    };
    Quantity?: number;
}