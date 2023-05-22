export interface Product {
    product_ID: number;
    product_Name: string;
    product_Description: string;
    product_Price:number;
    product_category_ID:number;
    productCategory:{
        product_category_ID:number;
        name:string;
        description:string;
    };
    product_type_ID:number;
        productType:{
        product_type_ID:number;
        name:string;
        description:string;
    };
}