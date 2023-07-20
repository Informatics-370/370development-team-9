export interface Trailer {
    trailerID: string;
    trailer_License: String;
    model:String;
    weight: number;
    trailer_Status_ID:string;
    trailerStatus:{
        trailer_Status_ID:string;
        status:string;
        description:string;
    };
    trailer_Type_ID:string;
    trailerType:{
        trailer_Type_ID:string;
        name:string;
        description:string;
    };
}