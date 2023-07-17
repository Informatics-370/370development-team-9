export interface Product {
    product_ID: number;
    product_Name: string;
    product_Description: string;
    product_Price:number;
    product_Category_ID:number;
    productCategory:{
        product_Category_ID:number;
        name:string;
        description:string;
    };
    product_Type_ID:number;
        productType:{
        product_Type_ID:number;
        name:string;
        description:string;
    };
    Quantity?: number;
}