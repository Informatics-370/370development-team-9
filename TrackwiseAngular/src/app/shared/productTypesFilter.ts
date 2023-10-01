export interface ProductTypeFilter{
    product_Type_ID:string;
    name:string;
    description:string;

    product_Category:{
        product_Category_ID:string;
        name:string;
        description:string;
    };
}