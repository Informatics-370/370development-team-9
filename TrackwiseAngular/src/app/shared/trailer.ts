export interface Trailer {
    trailerID: number;
    trailer_License: String;
    model:String;
    weight: number;
    trailer_Status_ID:number;
    trailerStatus:{
        trailer_Status_ID:number;
        status:string;
        description:string;
    };
    trailer_Type_ID:number;
    trailerType:{
        trailer_Type_ID:number;
        name:string;
        description:string;
    };
}