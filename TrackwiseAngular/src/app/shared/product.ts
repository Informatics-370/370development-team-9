export interface Product {
    product_ID: number;
    product_name: string;
    description: string;
    price:number;
    product_category_ID:number;
    productCategory:{
        product_category_ID:number;
        name:string;
        description:string;
    };
}