export interface ProductCategoriesFilter{
    product_Category_ID:string;
    name:string;
    description:string;

    product_Type:{
        product_Type_ID:string;
        name:string;
        description:string;
    };
}