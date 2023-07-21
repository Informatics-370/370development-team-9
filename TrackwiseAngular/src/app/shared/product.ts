export interface Product {
    product_ID: string;
    product_Name: string;
    product_Description: string;
    product_Price:number;
    quantity:number;
    cartQuantity?:number;

    productType:{
        product_Type_ID:string;
        name:string;
        description:string;
    };

    productCategory:{
        product_Category_ID:string;
        name:string;
        description:string;
    };
}

export interface ProductTypes{
    product_Type_ID: string;
    name: string;
    description: string;
}

export interface ProductCategories{
    product_Category_ID: string;
    name: string;
    description: string;
}